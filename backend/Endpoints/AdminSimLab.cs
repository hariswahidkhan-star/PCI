using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using PCI.Backend.Core;
using PCI.Backend.Data;

namespace PCI.Backend.Endpoints;

/// <summary>
/// Admin Console → Simulation Lab (Phase 1 foundation). A read-only operator view of the guided-lab /
/// scenario catalogue — every scenario across all statuses (draft, published, suspended, archived), its
/// competencies and interactivity, and how much practice each has seen (attempt + completion counts).
/// Authoring / publishing lifecycle arrives in a later increment; this gives operators visibility now.
///
/// Gated by the dedicated 'sim_lab' permission (least privilege — a simulation author no longer needs full
/// marketing-'content' rights to manage the Lab). 'content' is grandfathered to 'sim_lab' in Rbac.PermsFor so
/// no existing operator loses access. Never exposes student-identifying data — only per-scenario aggregates.
/// </summary>
public static class AdminSimLab
{
    public static void Map(WebApplication app, Db db, Action<long?, string, string?> log,
        Func<HttpRequest, string, Func<AdminCtx, IResult>, IResult> gate)
    {
        // ---- authoring templates: one known-good starter config per engine task ----
        // The Studio's config textarea is otherwise a blank page against 18 different engine contracts.
        // Every template is proven publishable by SimTemplatesTests, so "start from template" always yields
        // a draft that passes the validator before the author has written a word.
        app.MapGet("/api/admin/lab/templates", (HttpRequest req) => gate(req, "sim_lab", _ =>
            Results.Json(new
            {
                rows = SimTemplates.All.Select(t => new { task = t.Task, label = t.Label, hint = t.Hint, config = t.Config }),
            })));

        // ---- scenario catalogue (all statuses) + per-scenario practice aggregates ----
        app.MapGet("/api/admin/lab/scenarios", (HttpRequest req) => gate(req, "sim_lab", _ =>
        {
            // Per-scenario attempt + completion counts (aggregate only — no student identity).
            var stats = new Dictionary<long, (long attempts, long completed)>();
            foreach (var r in db.Query(@"SELECT scenario_id, COUNT(*) n,
                    SUM(CASE WHEN status IN ('completed','passed','failed') THEN 1 ELSE 0 END) c
                FROM simulation_attempts GROUP BY scenario_id"))
                stats[H.L(r["scenario_id"])] = (H.L(r["n"]), H.L(r["c"]));

            var rows = new List<object>();
            var published = 0;
            var now = DateTime.UtcNow;
            foreach (var s in db.Query(@"SELECT id,scenario_code,title,kind,industry,difficulty,est_minutes,
                    competencies_json,status,review_state,version,sort_order,config_json,updated_at,review_due,expires_at
                FROM simulation_scenarios ORDER BY sort_order ASC, id ASC"))
            {
                var id = H.L(s["id"]);
                var status = H.Str(s["status"]);
                if (status == "published") published++;
                var has = stats.TryGetValue(id, out var st);
                // Review-due / expiry governance (§13): computed against "now" from the two authored dates.
                var gov = SimGovernance.Evaluate(
                    s.TryGetValue("review_due", out var rdv) ? H.Str(rdv) : null,
                    s.TryGetValue("expires_at", out var exv) ? H.Str(exv) : null, now);
                rows.Add(new
                {
                    id,
                    scenario_code = H.Str(s["scenario_code"]),
                    title = H.Str(s["title"]),
                    kind = H.Str(s["kind"]),
                    industry = H.Str(s["industry"]),
                    difficulty = H.Str(s["difficulty"]),
                    est_minutes = H.L(s["est_minutes"]),
                    competencies = ParseArray(H.Str(s["competencies_json"])),
                    status,
                    review_state = s.TryGetValue("review_state", out var rvs) ? H.Str(rvs) : "draft",
                    version = H.L(s["version"]),
                    interactive = !string.IsNullOrWhiteSpace(H.Str(s["config_json"])),
                    attempts = has ? st.attempts : 0,
                    completed = has ? st.completed : 0,
                    updated_at = H.Str(s["updated_at"]),
                    review_due = gov.ReviewDue,
                    expires_at = gov.ExpiresAt,
                    governance = gov.Code,
                    days_to_review = gov.DaysToReview,
                    days_to_expiry = gov.DaysToExpiry,
                });
            }
            return Results.Json(new { rows, total = rows.Count, published });
        }));

        // ---- content-quality validation for one scenario (§14 publication gate) ----
        // Runs the deterministic SimContent validator: metadata completeness, retired-name check, and the
        // reference-solver pass (every asked measure must resolve through the engine). Read-only.
        app.MapGet("/api/admin/lab/scenarios/{id}/validate", (HttpRequest req, long id) => gate(req, "sim_lab", _ =>
        {
            var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
            if (s is null) return Results.NotFound(new { error = "not_found" });

            var input = InputFrom(s);
            var issues = SimContent.Validate(input, OtherCodes(db, id));
            return Results.Json(new
            {
                id,
                scenario_code = input.ScenarioCode,
                review_state = s.TryGetValue("review_state", out var rs) ? H.Str(rs) : "draft",
                publishable = SimContent.Publishable(issues),
                errors = issues.Count(i => i.Severity == SimContent.Severity.Error),
                warnings = issues.Count(i => i.Severity == SimContent.Severity.Warning),
                issues = issues.Select(i => new { severity = i.Severity.ToString().ToLowerInvariant(), code = i.Code, message = i.Message }),
            });
        }));

        // ---- deterministic manifest export for one scenario version (§5B.4). Read-only: content +
        //      governance state + the live validation verdict, checksummed over the graded content alone.
        //      Byte-identical on every call for the same content — there is no export timestamp, no exporter
        //      identity, no reviewer identity and no attempt counts (see SimManifest for why each is out). ----
        app.MapGet("/api/admin/lab/scenarios/{id}/manifest", (HttpContext ctx, long id) => gate(ctx.Request, "sim_lab", adm =>
        {
            var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
            if (s is null) return Results.NotFound(new { error = "not_found" });

            var json = SimManifest.Build(s, SimContent.Validate(InputFrom(s), OtherCodes(db, id)));
            var code = H.Str(s["scenario_code"]);
            var version = H.L(s["version"]);
            // FileName reduces the operator-authored code to a safe alphabet — a quote or newline in a
            // scenario code must never be able to forge a response header.
            ctx.Response.Headers["Content-Disposition"] = $"attachment; filename=\"{SimManifest.FileName(code, version)}\"";
            log(adm.Id, "sim_scenario_export", $"{code} v{version}");
            return Results.Text(json, "application/json", Encoding.UTF8);
        }));

        // ---- whole-catalogue manifest bundle (§5B.6). One deterministic file for backup or migration,
        //      instead of exporting scenarios one at a time. `status` narrows it (default: everything);
        //      ordering is by scenario_code so two environments holding the same content agree. ----
        app.MapGet("/api/admin/lab/manifest-bundle", (HttpContext ctx, string? status) => gate(ctx.Request, "sim_lab", adm =>
        {
            // Validated against the operational status vocabulary rather than sanitised, so the value is
            // safe by construction both in the query and in the response header.
            var wanted = (status ?? "").Trim().ToLowerInvariant();
            if (wanted.Length > 0 && wanted is not ("draft" or "published" or "suspended" or "archived"))
                return Results.Json(new { error = "bad_status", message = "Use draft, published, suspended or archived." }, statusCode: 400);

            var rows = wanted.Length == 0
                ? db.Query("SELECT * FROM simulation_scenarios")
                : db.Query("SELECT * FROM simulation_scenarios WHERE status=?", wanted);

            var all = new List<(Dictionary<string, object?>, IReadOnlyList<SimContent.Issue>)>();
            foreach (var s in rows)
                all.Add((s, SimContent.Validate(InputFrom(s), OtherCodes(db, H.L(s["id"])))));

            var json = SimManifest.Bundle(all);
            var name = wanted.Length == 0 ? "catalogue" : wanted;
            ctx.Response.Headers["Content-Disposition"] = $"attachment; filename=\"pci-simulation-{name}.pcisim-bundle.json\"";
            log(adm.Id, "sim_bundle_export", $"{all.Count} scenario(s){(wanted.Length > 0 ? $" · {wanted}" : "")}");
            return Results.Text(json, "application/json", Encoding.UTF8);
        }));

        // ---- import a manifest as a NEW DRAFT (§5B.5 — the other half of manifest I/O). Verifies the
        //      envelope and recomputed content checksum, then lands the scenario in draft.
        //
        //      An import can never publish. Whatever lifecycle the file claims, the row is written at
        //      draft/draft and must walk the full review workflow here — and because the importing admin is
        //      recorded as the author, maker-checker still forces a different admin to approve it. Governance
        //      dates are NOT carried either: a review-due or expiry from the source environment is a
        //      statement about that environment's schedule, and silently importing one could expire content
        //      the moment it lands. ----
        // ---- import a whole catalogue bundle (§5B.8). Integrity is all-or-nothing: a bundle whose
        //      checksum does not reconcile imports NOTHING, because if one entry was altered in transit
        //      there is no reason to trust the rest. A code that is already taken is a different matter —
        //      that is this environment's state, not the file's integrity — so those entries are skipped
        //      and named, which also makes re-running an interrupted migration safe. ----
        app.MapPost("/api/admin/lab/scenarios/import-bundle", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var bundle = H.GetEl(b, "bundle") is { ValueKind: JsonValueKind.Object } el
                    ? JsonNode.Parse(el.GetRawText()) as JsonObject : null;

                var verdict = SimManifest.VerifyBundle(bundle, out var entries);
                if (verdict != SimManifest.Reject.None)
                    return Results.Json(new { error = SimManifest.Code(verdict), message = RejectMessage(verdict, bundleWord: true) },
                        statusCode: verdict == SimManifest.Reject.ChecksumMismatch ? 409 : 400);

                var imported = new List<object>();
                var skipped = new List<object>();
                foreach (var e in entries)
                {
                    if (db.QueryOne("SELECT id FROM simulation_scenarios WHERE scenario_code=?", e.Code) is not null)
                    {
                        skipped.Add(new { scenario_code = e.Code, reason = "duplicate_code" });
                        continue;
                    }
                    var newId = InsertImported(db, e.Row, e.Code, adm.Id);
                    var stored = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", newId)!;
                    var found = SimContent.Validate(InputFrom(stored), OtherCodes(db, newId));
                    imported.Add(new
                    {
                        id = newId, scenario_code = e.Code, checksum = e.Checksum, imported_version = e.Version,
                        publishable = SimContent.Publishable(found),
                        errors = found.Count(i => i.Severity == SimContent.Severity.Error),
                        warnings = found.Count(i => i.Severity == SimContent.Severity.Warning),
                    });
                }

                log(adm.Id, "sim_bundle_import", $"{imported.Count} imported · {skipped.Count} skipped of {entries.Count}");
                return Results.Json(new
                {
                    total = entries.Count, imported_count = imported.Count, skipped_count = skipped.Count,
                    imported, skipped,
                });
            });
        });

        app.MapPost("/api/admin/lab/scenarios/import", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var manifest = H.GetEl(b, "manifest") is { ValueKind: JsonValueKind.Object } mEl
                    ? JsonNode.Parse(mEl.GetRawText()) as JsonObject : null;

                var verdict = SimManifest.Verify(manifest, out var row, out var checksum);
                if (verdict != SimManifest.Reject.None)
                    return Results.Json(new { error = SimManifest.Code(verdict), message = RejectMessage(verdict, bundleWord: false) },
                        statusCode: verdict == SimManifest.Reject.ChecksumMismatch ? 409 : 400);

                // The operator may land the import under a different code (importing alongside a scenario
                // that already owns the original code).
                var code = (H.GetS(b, "as_code") ?? H.Str(row["scenario_code"]) ?? "").Trim();
                if (code.Length == 0) return Results.Json(new { error = "bad_manifest" }, statusCode: 400);
                if (db.QueryOne("SELECT id FROM simulation_scenarios WHERE scenario_code=?", code) is not null)
                    return Results.Json(new { error = "duplicate_code", message = "A scenario with this code already exists — import it under a different code." }, statusCode: 409);

                var id = InsertImported(db, row, code, adm.Id);

                // Re-run the §14 validator here: content that was publishable in the source environment may
                // not be publishable in this one (a retired name, a colliding code, a task this build cannot
                // solve), and the operator should see that immediately rather than at the approval gate.
                var stored = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id)!;
                var issues = SimContent.Validate(InputFrom(stored), OtherCodes(db, id));
                log(adm.Id, "sim_scenario_import", $"{code} · #{id} · {checksum[..12]}");
                return Results.Json(new
                {
                    id, scenario_code = code, review_state = "draft", status = "draft", checksum,
                    imported_version = H.L(row["version"]),
                    publishable = SimContent.Publishable(issues),
                    errors = issues.Count(i => i.Severity == SimContent.Severity.Error),
                    warnings = issues.Count(i => i.Severity == SimContent.Severity.Warning),
                    issues = issues.Select(i => new { severity = i.Severity.ToString().ToLowerInvariant(), code = i.Code, message = i.Message }),
                });
            });
        });

        // ---- create a DRAFT scenario (§13 authoring). Records the author for maker-checker; starts in the
        //      draft review state and is never served to students until it walks the review workflow. ----
        app.MapPost("/api/admin/lab/scenarios", async (HttpContext ctx) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var code = (H.GetS(b, "scenario_code") ?? "").Trim();
                var title = (H.GetS(b, "title") ?? "").Trim();
                if (code.Length == 0 || title.Length == 0)
                    return Results.Json(new { error = "bad_input", message = "scenario_code and title are required." }, statusCode: 400);
                if (db.QueryOne("SELECT id FROM simulation_scenarios WHERE scenario_code=?", code) is not null)
                    return Results.Json(new { error = "duplicate_code" }, statusCode: 409);

                var competencies = H.GetEl(b, "competencies") is { ValueKind: JsonValueKind.Array } ca
                    ? JsonSerializer.Serialize(ca.EnumerateArray().Select(e => e.GetString()).Where(x => !string.IsNullOrWhiteSpace(x)))
                    : (H.GetS(b, "competencies_json") ?? "[]");
                var config = H.GetEl(b, "config_json") is { ValueKind: JsonValueKind.Object } cj ? cj.GetRawText() : H.GetS(b, "config_json");
                long? certId = H.GetNum(b, "certification_id") is { } cn ? (long)cn : null;

                var id = db.ExecuteReturningId(@"INSERT INTO simulation_scenarios
                    (scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,certification_id,
                     summary,config_json,status,review_state,version,synthetic_declared,authored_by,created_by)
                    VALUES(?,?,?,?,?,?,?,?,?,?, 'draft','draft', 1, ?, ?, ?)",
                    code, title, (H.GetS(b, "kind") ?? "scenario").Trim(), (H.GetS(b, "industry") ?? "").Trim(),
                    (H.GetS(b, "difficulty") ?? "foundation").Trim(), (int)(H.GetNum(b, "est_minutes") ?? 15),
                    competencies, certId, H.GetS(b, "summary"), config,
                    Truthy(H.GetEl(b, "synthetic_declared")) ? 1 : 0, adm.Id, adm.Id);
                log(adm.Id, "sim_scenario_create", $"{code} · #{id}");
                return Results.Json(new { id, scenario_code = code, review_state = "draft", status = "draft" });
            });
        });

        // ---- advance a scenario through the review workflow (§13). Structural moves are gated by
        //      SimReview; approve/publish additionally require the §14 validator to pass; approval enforces
        //      maker-checker (approver != author). Keeps the operational status in sync and audits each move.
        app.MapPost("/api/admin/lab/scenarios/{id}/review", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
                if (s is null) return Results.NotFound(new { error = "not_found" });
                var from = (s.TryGetValue("review_state", out var rs0) ? H.Str(rs0) : null) ?? "draft";
                var to = (H.GetS(b, "to") ?? "").Trim();
                if (!SimReview.IsState(to)) return Results.Json(new { error = "bad_state" }, statusCode: 400);
                if (!SimReview.CanTransition(from, to))
                    return Results.Json(new { error = "bad_transition", from, to }, statusCode: 409);

                if (SimReview.RequiresPublishable(to))
                {
                    var issues = SimContent.Validate(InputFrom(s), OtherCodes(db, id));
                    if (!SimContent.Publishable(issues))
                        return Results.Json(new
                        {
                            error = "not_publishable",
                            errors = issues.Count(i => i.Severity == SimContent.Severity.Error),
                            issues = issues.Where(i => i.Severity == SimContent.Severity.Error)
                                .Select(i => new { code = i.Code, message = i.Message }),
                        }, statusCode: 409);
                }
                if (SimReview.RequiresDifferentChecker(to))
                {
                    var author = s.TryGetValue("authored_by", out var ab) && ab is not null ? H.L(ab) : 0;
                    if (author != 0 && author == adm.Id)
                        return Results.Json(new { error = "maker_checker", message = "The approver must be different from the author." }, statusCode: 409);
                }

                db.Execute("UPDATE simulation_scenarios SET review_state=?, updated_at=datetime('now') WHERE id=?", to, id);
                // Whoever advances OUT of a review stage signs it off.
                if (from == SimReview.CalcReview) db.Execute("UPDATE simulation_scenarios SET calc_reviewed_by=? WHERE id=?", adm.Id, id);
                else if (from == SimReview.LearningReview) db.Execute("UPDATE simulation_scenarios SET learning_reviewed_by=? WHERE id=?", adm.Id, id);
                else if (from == SimReview.SafetyReview) db.Execute("UPDATE simulation_scenarios SET safety_reviewed_by=? WHERE id=?", adm.Id, id);
                if (to == SimReview.Approved) db.Execute("UPDATE simulation_scenarios SET approved_by=?, approved_at=datetime('now') WHERE id=?", adm.Id, id);
                if (to == SimReview.Published) db.Execute("UPDATE simulation_scenarios SET status='published', published_at=COALESCE(published_at, datetime('now')) WHERE id=?", id);
                else if (to == SimReview.Retired) db.Execute("UPDATE simulation_scenarios SET status='archived' WHERE id=?", id);
                else if (to == SimReview.Draft) db.Execute("UPDATE simulation_scenarios SET status='draft' WHERE id=?", id);

                log(adm.Id, "sim_scenario_review", $"{H.Str(s["scenario_code"])} {from}→{to}");
                return Results.Json(new { id, review_state = to, from });
            });
        });

        // ---- edit a scenario's content — allowed only while it is still in authoring/review (§18: an
        //      approved or published version is immutable; to change it, revise into a new version). ----
        app.MapPatch("/api/admin/lab/scenarios/{id}", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
                if (s is null) return Results.NotFound(new { error = "not_found" });
                var state = (s.TryGetValue("review_state", out var rs) ? H.Str(rs) : null) ?? "draft";
                if (state is SimReview.Approved or SimReview.Published or SimReview.Retired)
                    return Results.Json(new { error = "immutable", message = "Approved/published/retired scenarios are frozen — revise into a new version to change them." }, statusCode: 409);

                var sets = new List<string>(); var vals = new List<object?>();
                void SetIf(string key, string col, Func<JsonElement, object?> map)
                {
                    if (H.GetEl(b, key) is { } el) { sets.Add($"{col}=?"); vals.Add(map(el)); }
                }
                SetIf("title", "title", e => e.GetString());
                SetIf("summary", "summary", e => e.GetString());
                SetIf("kind", "kind", e => e.GetString());
                SetIf("industry", "industry", e => e.GetString());
                SetIf("difficulty", "difficulty", e => e.GetString());
                SetIf("est_minutes", "est_minutes", e => e.ValueKind == JsonValueKind.Number ? e.GetInt32() : 15);
                SetIf("competencies", "competencies_json", e => e.ValueKind == JsonValueKind.Array
                    ? JsonSerializer.Serialize(e.EnumerateArray().Select(x => x.GetString()).Where(x => !string.IsNullOrWhiteSpace(x)))
                    : e.GetString());
                SetIf("certification_id", "certification_id", e => e.ValueKind == JsonValueKind.Number ? (object?)e.GetInt64() : null);
                SetIf("config_json", "config_json", e => e.ValueKind == JsonValueKind.Object ? e.GetRawText() : e.GetString());
                SetIf("objectives", "objectives_json", e => e.ValueKind == JsonValueKind.Array ? e.GetRawText() : e.GetString());
                SetIf("provenance", "provenance", e => e.GetString());
                SetIf("disclaimers", "disclaimers", e => e.GetString());
                SetIf("worked_solution", "worked_solution", e => e.GetString());
                SetIf("synthetic_declared", "synthetic_declared", e => Truthy(el: e) ? 1 : 0);
                if (sets.Count == 0) return Results.Json(new { error = "no_fields" }, statusCode: 400);

                vals.Add(id);
                db.Execute($"UPDATE simulation_scenarios SET {string.Join(",", sets)}, updated_at=datetime('now') WHERE id=?", vals.ToArray());
                log(adm.Id, "sim_scenario_edit", $"{H.Str(s["scenario_code"])} · {sets.Count} field(s)");
                var issues = SimContent.Validate(InputFrom(db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id)!), OtherCodes(db, id));
                return Results.Json(new { id, updated = sets.Count, publishable = SimContent.Publishable(issues) });
            });
        });

        // ---- revise a scenario into a NEW draft version (§13 controlled clone/revision). The source stays
        //      untouched (immutable if published); the copy starts at draft with version+1 and a new code. ----
        app.MapPost("/api/admin/lab/scenarios/{id}/revise", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var s = db.QueryOne("SELECT * FROM simulation_scenarios WHERE id=?", id);
                if (s is null) return Results.NotFound(new { error = "not_found" });
                var newCode = (H.GetS(b, "new_code") ?? "").Trim();
                if (newCode.Length == 0) return Results.Json(new { error = "bad_input", message = "new_code is required." }, statusCode: 400);
                if (db.QueryOne("SELECT id FROM simulation_scenarios WHERE scenario_code=?", newCode) is not null)
                    return Results.Json(new { error = "duplicate_code" }, statusCode: 409);

                var newId = db.ExecuteReturningId(@"INSERT INTO simulation_scenarios
                    (scenario_code,title,kind,industry,difficulty,est_minutes,competencies_json,certification_id,summary,
                     config_json,objectives_json,provenance,disclaimers,worked_solution,status,review_state,version,
                     synthetic_declared,authored_by,created_by)
                    SELECT ?, title,kind,industry,difficulty,est_minutes,competencies_json,certification_id,summary,
                     config_json,objectives_json,provenance,disclaimers,worked_solution,'draft','draft', version+1,
                     synthetic_declared, ?, ? FROM simulation_scenarios WHERE id=?",
                    newCode, adm.Id, adm.Id, id);
                log(adm.Id, "sim_scenario_revise", $"{H.Str(s["scenario_code"])} → {newCode} (v{H.L(s["version"]) + 1})");
                return Results.Json(new { id = newId, scenario_code = newCode, review_state = "draft", status = "draft", revised_from = id });
            });
        });

        // ---- set the review-due / expiry governance dates (§13). These are governance METADATA, not
        //      scenario content, so they may be set at any lifecycle state (including published) without
        //      breaching published-immutability. Either date may be cleared by sending an empty/null value;
        //      a present-but-unparseable date is rejected before it can reach the column. ----
        app.MapPatch("/api/admin/lab/scenarios/{id}/governance", async (HttpContext ctx, long id) =>
        {
            var b = await H.Body(ctx.Request);
            return gate(ctx.Request, "sim_lab", adm =>
            {
                var s = db.QueryOne("SELECT id,scenario_code FROM simulation_scenarios WHERE id=?", id);
                if (s is null) return Results.NotFound(new { error = "not_found" });

                var sets = new List<string>(); var vals = new List<object?>(); var touched = new List<string>();
                foreach (var key in new[] { "review_due", "expires_at" })
                {
                    if (H.GetEl(b, key) is not { } el) continue;   // key absent → leave column untouched
                    var raw = el.ValueKind == JsonValueKind.String ? el.GetString() : null;
                    var val = string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();   // blank/null clears
                    if (val is not null && !SimGovernance.IsValidDate(val))
                        return Results.Json(new { error = "bad_date", field = key, message = "Use YYYY-MM-DD (or a full timestamp)." }, statusCode: 400);
                    sets.Add($"{key}=?"); vals.Add(val); touched.Add(key);
                }
                if (sets.Count == 0)
                    return Results.Json(new { error = "no_fields", message = "Provide review_due and/or expires_at." }, statusCode: 400);

                vals.Add(id);
                db.Execute($"UPDATE simulation_scenarios SET {string.Join(",", sets)}, updated_at=datetime('now') WHERE id=?", vals.ToArray());
                log(adm.Id, "sim_scenario_governance", $"{H.Str(s["scenario_code"])} · {string.Join("+", touched)}");
                var row = db.QueryOne("SELECT review_due,expires_at FROM simulation_scenarios WHERE id=?", id)!;
                var gov = SimGovernance.Evaluate(H.Str(row["review_due"]), H.Str(row["expires_at"]), DateTime.UtcNow);
                return Results.Json(new
                {
                    id, review_due = gov.ReviewDue, expires_at = gov.ExpiresAt,
                    governance = gov.Code, days_to_review = gov.DaysToReview, days_to_expiry = gov.DaysToExpiry,
                });
            });
        });
    }

    static string RejectMessage(SimManifest.Reject r, bool bundleWord)
    {
        var noun = bundleWord ? "bundle" : "manifest";
        return r switch
        {
            SimManifest.Reject.WrongKind => bundleWord
                ? "This file is not a PCI simulation catalogue bundle."
                : "This file is not a PCI simulation scenario manifest.",
            SimManifest.Reject.UnsupportedVersion => $"This {noun} was written by a newer version of the platform.",
            SimManifest.Reject.ChecksumMismatch => bundleWord
                ? "The bundle's content does not match its checksum — it was modified or truncated, so none of it was imported."
                : "The manifest's content does not match its checksum — it was modified or truncated.",
            _ => bundleWord
                ? "The bundle is missing the fields a catalogue needs."
                : "The manifest is missing the fields a scenario needs.",
        };
    }

    /// <summary>
    /// The single INSERT both import paths use, so the two can never disagree about what an import is.
    /// Three things are forced here rather than taken from the file: the row lands as a DRAFT (nothing in
    /// a file may grant published state), the importing admin is the author (so maker-checker still needs
    /// a second pair of eyes), and version starts at 1 (version numbers are a per-environment lineage, not
    /// a property of the content — an imported v7 becomes this environment's v1, pinned by its checksum).
    /// Governance dates are deliberately not carried: they describe the source environment's schedule.
    /// </summary>
    static long InsertImported(Db db, Dictionary<string, object?> row, string code, long adminId) =>
        db.ExecuteReturningId(@"INSERT INTO simulation_scenarios
            (scenario_code,title,kind,industry,project_type,difficulty,est_minutes,competencies_json,
             certification_id,summary,brief,config_json,objectives_json,provenance,disclaimers,
             worked_solution,status,review_state,version,synthetic_declared,authored_by,created_by)
            VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?, 'draft','draft', 1, ?, ?, ?)",
            code, H.Str(row["title"]), H.Str(row["kind"]) ?? "scenario", H.Str(row["industry"]),
            H.Str(row["project_type"]), H.Str(row["difficulty"]) ?? "foundation",
            (int)H.L(row["est_minutes"]), H.Str(row["competencies_json"]),
            row["certification_id"], H.Str(row["summary"]), H.Str(row["brief"]),
            H.Str(row["config_json"]), H.Str(row["objectives_json"]), H.Str(row["provenance"]),
            H.Str(row["disclaimers"]), H.Str(row["worked_solution"]),
            H.L(row["synthetic_declared"]) == 1 ? 1 : 0, adminId, adminId);

    static SimContent.ScenarioInput InputFrom(Dictionary<string, object?> s) => new(
        H.Str(s["scenario_code"]) ?? "", H.Str(s["title"]), H.Str(s["summary"]), H.Str(s["difficulty"]),
        s.TryGetValue("certification_id", out var cv) && cv is not null ? H.L(cv) : (long?)null,
        H.Str(s["competencies_json"]), H.Str(s["config_json"]),
        s.TryGetValue("synthetic_declared", out var sd) && sd is not null && H.L(sd) == 1);

    static List<string> OtherCodes(Db db, long id)
    {
        var others = new List<string>();
        foreach (var r in db.Query("SELECT scenario_code FROM simulation_scenarios WHERE id<>?", id))
            others.Add(H.Str(r["scenario_code"]) ?? "");
        return others;
    }

    static bool Truthy(JsonElement? el) => el is { } e && (e.ValueKind == JsonValueKind.True
        || (e.ValueKind == JsonValueKind.String && e.GetString() is "1" or "true")
        || (e.ValueKind == JsonValueKind.Number && e.TryGetInt32(out var i) && i != 0));

    static string[] ParseArray(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
        try { return JsonSerializer.Deserialize<string[]>(json) ?? Array.Empty<string>(); }
        catch { return Array.Empty<string>(); }
    }
}
