using System.Text.Json;

namespace PCI.Backend.Core;

/// <summary>
/// PCI World — Project Intelligence taxonomy, classification and Year-1 plan
/// (docs/pciworld/PROJECT_INTELLIGENCE.md).
///
/// "PCI Project Intelligence" is the premium professional-practice programme built ON the existing
/// PCI World challenge engine — same challenges, immutable versions, rotation ledger, attempts and
/// admin realm. This module adds the governed taxonomy that the programme reports coverage against:
/// eight experience types, twelve competency domains, six lifecycle stages, ten sectors, six
/// interaction patterns and four duration bands. It deliberately owns NO scoring, no attempt state
/// and no serving authority: taxonomy is catalogue metadata on the working copy
/// (pciworld_challenges.pi_*), never part of the immutable replay snapshot.
///
/// It also owns two publication gates:
///   • the premium-language gate — learner-facing text must never describe the programme as a
///     game/puzzle/trivia product;
///   • the Year-1 manifest loader — the 420-slot governed content plan
///     (backend/content/project-intelligence/year-1/) that CI verifies against the approved
///     distribution tables, so coverage can never be claimed before it is real.
/// </summary>
public static class WorldIntelligence
{
    // ── Approved vocabularies. Snake_case tokens are the stored/API values; labels are the only
    //    learner-facing strings. Every table the Year-1 plan is verified against uses these keys. ──

    public static readonly IReadOnlyDictionary<string, string> ExperienceTypes = new Dictionary<string, string>
    {
        ["daily_decision"] = "Daily Decision",
        ["project_rescue"] = "Project Rescue",
        ["risk_room"] = "Risk Room",
        ["schedule_strategy"] = "Schedule Strategy",
        ["cost_value"] = "Cost & Value",
        ["stakeholder_dilemma"] = "Stakeholder Dilemma",
        ["logic_sequence"] = "Logic & Sequence",
        ["executive_mission"] = "Executive Mission",
    };

    public static readonly IReadOnlyDictionary<string, string> Domains = new Dictionary<string, string>
    {
        ["integration_governance"] = "Integration, strategy and governance",
        ["scope_requirements"] = "Scope and requirements",
        ["schedule_planning"] = "Schedule and planning",
        ["cost_commercial"] = "Cost, value and commercial management",
        ["risk_uncertainty"] = "Risk and uncertainty",
        ["quality_assurance"] = "Quality and assurance",
        ["resources_leadership"] = "Resources, teams and leadership",
        ["stakeholders_communication"] = "Stakeholders and communication",
        ["procurement_contracts"] = "Procurement and contracts",
        ["change_claims_recovery"] = "Change, claims and recovery",
        ["safety_sustainability"] = "Safety, sustainability and social value",
        ["digital_data_ai"] = "Digital delivery, data and responsible AI",
    };

    public static readonly IReadOnlyDictionary<string, string> LifecycleStages = new Dictionary<string, string>
    {
        ["concept_business_case"] = "Concept and business case",
        ["definition_planning"] = "Definition and planning",
        ["procurement_mobilization"] = "Procurement and mobilisation",
        ["execution_control"] = "Execution and control",
        ["commissioning_handover"] = "Commissioning and handover",
        ["closeout_lessons"] = "Closeout and lessons",
    };

    public static readonly IReadOnlyDictionary<string, string> Sectors = new Dictionary<string, string>
    {
        ["cross_sector"] = "Cross-sector",
        ["construction_infrastructure"] = "Construction and infrastructure",
        ["energy_utilities"] = "Energy and utilities",
        ["technology_digital"] = "Technology and digital",
        ["manufacturing_industrial"] = "Manufacturing and industrial",
        ["transport_logistics"] = "Transport and logistics",
        ["public_sector"] = "Public sector",
        ["healthcare_life_sciences"] = "Healthcare and life sciences",
        ["climate_sustainability"] = "Climate and sustainability",
        ["professional_services_other"] = "Professional services, events and other",
    };

    public static readonly IReadOnlyDictionary<string, string> Interactions = new Dictionary<string, string>
    {
        ["single_decision"] = "Single professional decision",
        ["order_rank"] = "Order or rank",
        ["numeric_calculation"] = "Numeric calculation and interpretation",
        ["multi_stage_decision"] = "Predefined multi-stage decision",
        ["evidence_diagnosis"] = "Diagnosis or evidence analysis",
        ["negotiation_communication"] = "Negotiation or communication response",
    };

    public static readonly IReadOnlyDictionary<string, string> DurationBands = new Dictionary<string, string>
    {
        ["quick"] = "Quick: 5–7 minutes",
        ["standard"] = "Standard: 8–12 minutes",
        ["deep"] = "Deep: 13–20 minutes",
        ["capstone"] = "Capstone: 21–30 minutes",
    };

    public static readonly IReadOnlyDictionary<string, string> DifficultyBands = new Dictionary<string, string>
    {
        ["foundation"] = "Foundation",
        ["practitioner"] = "Practitioner",
        ["advanced"] = "Advanced",
        ["executive"] = "Executive",
    };

    /// <summary>Reporting band for an estimated duration (the four approved bands).</summary>
    public static string DurationBand(long minutes) =>
        minutes <= 7 ? "quick" : minutes <= 12 ? "standard" : minutes <= 20 ? "deep" : "capstone";

    /// <summary>Map the engine's five-value difficulty onto the four approved reporting bands.
    /// The stored challenge difficulty is untouched — this is a projection, never a rewrite.</summary>
    public static string DifficultyBand(string? difficulty) => difficulty switch
    {
        "foundation" or "developing" => "foundation",
        "professional" => "practitioner",
        "advanced" => "advanced",
        "expert" => "executive",
        _ => "foundation",
    };

    // ── Premium-language gate. The programme is professional practice: learner-facing copy must
    //    never call it a game. Applied to titles/hooks at validation and to the Year-1 plan in CI.
    //    The list is word-bounded and deliberately conservative — everyday words like "play" and
    //    "lives" would flag legitimate prose ("the plan plays out", "lives depend on it"). ──
    static readonly System.Text.RegularExpressions.Regex Banned = new(
        @"\b(game|games|gaming|mini-?games?|puzzle|puzzles|trivia|arcade|leaderboard|jackpot|entertainment)\b",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);

    public static List<string> LanguageIssues(params string?[] learnerFacingTexts)
    {
        var issues = new List<string>();
        foreach (var t in learnerFacingTexts)
        {
            if (string.IsNullOrWhiteSpace(t)) continue;
            foreach (System.Text.RegularExpressions.Match m in Banned.Matches(t))
                issues.Add($"banned term '{m.Value}' in: \"{(t.Length > 60 ? t[..60] + "…" : t)}\"");
        }
        return issues;
    }

    /// <summary>Vocabulary-membership errors for a full taxonomy assignment (empty = valid).</summary>
    public static List<string> ValidateMeta(string? type, string? domain, string? lifecycle, string? sector, string? interaction)
    {
        var errs = new List<string>();
        if (type is not null && !ExperienceTypes.ContainsKey(type)) errs.Add($"unknown experience_type '{type}'");
        if (domain is not null && !Domains.ContainsKey(domain)) errs.Add($"unknown primary_domain '{domain}'");
        if (lifecycle is not null && !LifecycleStages.ContainsKey(lifecycle)) errs.Add($"unknown lifecycle_stage '{lifecycle}'");
        if (sector is not null && !Sectors.ContainsKey(sector)) errs.Add($"unknown sector '{sector}'");
        if (interaction is not null && !Interactions.ContainsKey(interaction)) errs.Add($"unknown interaction '{interaction}'");
        return errs;
    }

    // ── Classification of the existing bank (the Section-2 audit, made executable). Every house
    //    challenge is assigned exactly one value per facet. The audit verdict for all 52 was
    //    CONDITIONALLY ACCEPTED; the D3 remediation (docs/pciworld/PROJECT_INTELLIGENCE.md) has
    //    since closed the hints gap — every legacy item now carries exactly three progressive
    //    hints behind the same leakage and language gates as the Year-1 pack, and the
    //    WC-BOQ-012 rate-label defect is fixed. Coach-boundary fields and review-by dates are
    //    consolidated under D5, where they were already a dependency. ──

    public sealed record Meta(string Type, string Domain, string Lifecycle, string Sector, string Interaction);

    public static readonly IReadOnlyDictionary<string, Meta> Classification = new Dictionary<string, Meta>
    {
        ["WC-EVM-001"] = new("cost_value", "cost_commercial", "execution_control", "transport_logistics", "numeric_calculation"),
        ["WC-SCH-002"] = new("schedule_strategy", "schedule_planning", "definition_planning", "technology_digital", "numeric_calculation"),
        ["WC-RSK-003"] = new("risk_room", "risk_uncertainty", "definition_planning", "climate_sustainability", "numeric_calculation"),
        ["WC-CSH-004"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation"),
        ["WC-CHG-005"] = new("daily_decision", "change_claims_recovery", "execution_control", "healthcare_life_sciences", "single_decision"),
        ["WC-PRG-006"] = new("schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation"),
        ["WC-ESC-007"] = new("schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "numeric_calculation"),
        ["WC-PRT-008"] = new("risk_room", "risk_uncertainty", "definition_planning", "transport_logistics", "numeric_calculation"),
        ["WC-TML-009"] = new("schedule_strategy", "schedule_planning", "execution_control", "public_sector", "numeric_calculation"),
        ["WC-AIA-010"] = new("daily_decision", "digital_data_ai", "execution_control", "technology_digital", "evidence_diagnosis"),
        ["WC-PRD-011"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation"),
        ["WC-BOQ-012"] = new("cost_value", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "numeric_calculation"),
        ["WC-RES-013"] = new("schedule_strategy", "resources_leadership", "definition_planning", "energy_utilities", "numeric_calculation"),
        ["WC-PRC-014"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "transport_logistics", "single_decision"),
        ["WC-PTF-015"] = new("executive_mission", "integration_governance", "concept_business_case", "cross_sector", "multi_stage_decision"),
        ["WC-DEC-016"] = new("daily_decision", "integration_governance", "concept_business_case", "manufacturing_industrial", "single_decision"),
        ["WC-DQA-017"] = new("daily_decision", "digital_data_ai", "execution_control", "technology_digital", "evidence_diagnosis"),
        ["WC-EVM-018"] = new("cost_value", "cost_commercial", "execution_control", "energy_utilities", "numeric_calculation"),
        ["WC-CSH-019"] = new("cost_value", "cost_commercial", "execution_control", "professional_services_other", "numeric_calculation"),
        ["WC-RSK-020"] = new("risk_room", "risk_uncertainty", "definition_planning", "energy_utilities", "numeric_calculation"),
        ["WC-PRT-021"] = new("risk_room", "risk_uncertainty", "definition_planning", "public_sector", "numeric_calculation"),
        ["WC-CPM-022"] = new("schedule_strategy", "schedule_planning", "definition_planning", "healthcare_life_sciences", "numeric_calculation"),
        ["WC-CHG-023"] = new("daily_decision", "change_claims_recovery", "execution_control", "public_sector", "single_decision"),
        ["WC-ESC-024"] = new("schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "numeric_calculation"),
        ["WC-TML-025"] = new("schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "numeric_calculation"),
        ["WC-WBS-026"] = new("logic_sequence", "scope_requirements", "definition_planning", "manufacturing_industrial", "numeric_calculation"),
        ["WC-CBS-027"] = new("cost_value", "cost_commercial", "execution_control", "transport_logistics", "numeric_calculation"),
        ["WC-PRG-028"] = new("schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation"),
        ["WC-AIA-029"] = new("daily_decision", "digital_data_ai", "execution_control", "technology_digital", "evidence_diagnosis"),
        ["WC-CAP-030"] = new("executive_mission", "integration_governance", "concept_business_case", "climate_sustainability", "multi_stage_decision"),
        ["WC-EVM-031"] = new("cost_value", "cost_commercial", "execution_control", "technology_digital", "numeric_calculation"),
        ["WC-CPM-032"] = new("schedule_strategy", "schedule_planning", "definition_planning", "healthcare_life_sciences", "numeric_calculation"),
        ["WC-RSK-033"] = new("risk_room", "risk_uncertainty", "definition_planning", "energy_utilities", "numeric_calculation"),
        ["WC-CSH-034"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation"),
        ["WC-BOQ-035"] = new("cost_value", "procurement_contracts", "procurement_mobilization", "transport_logistics", "numeric_calculation"),
        ["WC-PRD-036"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation"),
        ["WC-RES-037"] = new("schedule_strategy", "resources_leadership", "definition_planning", "energy_utilities", "numeric_calculation"),
        ["WC-CBS-038"] = new("cost_value", "cost_commercial", "execution_control", "healthcare_life_sciences", "numeric_calculation"),
        ["WC-PRT-039"] = new("risk_room", "risk_uncertainty", "definition_planning", "manufacturing_industrial", "numeric_calculation"),
        ["WC-CHG-040"] = new("daily_decision", "change_claims_recovery", "execution_control", "energy_utilities", "single_decision"),
        ["WC-ESC-041"] = new("schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation"),
        ["WC-TML-042"] = new("schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation"),
        ["WC-PFO-043"] = new("executive_mission", "integration_governance", "concept_business_case", "public_sector", "multi_stage_decision"),
        ["WC-DEC-044"] = new("daily_decision", "integration_governance", "concept_business_case", "manufacturing_industrial", "single_decision"),
        ["WC-DQ-045"] = new("daily_decision", "digital_data_ai", "execution_control", "transport_logistics", "evidence_diagnosis"),
        ["WC-PRC-046"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "manufacturing_industrial", "single_decision"),
        ["WC-PRG-047"] = new("schedule_strategy", "schedule_planning", "execution_control", "public_sector", "numeric_calculation"),
        ["WC-WBS-048"] = new("logic_sequence", "scope_requirements", "definition_planning", "technology_digital", "numeric_calculation"),
        ["WC-EVM-049"] = new("cost_value", "cost_commercial", "execution_control", "energy_utilities", "numeric_calculation"),
        ["WC-CPM-050"] = new("schedule_strategy", "schedule_planning", "definition_planning", "transport_logistics", "numeric_calculation"),
        ["WC-TRC-051"] = new("daily_decision", "quality_assurance", "execution_control", "professional_services_other", "evidence_diagnosis"),
        ["WC-TRC-052"] = new("daily_decision", "quality_assurance", "execution_control", "healthcare_life_sciences", "evidence_diagnosis"),

        // ── Year-1 January authored pack (WorldIntelligencePack) — each item is authored TO its
        //    plan slot, so these assignments are fixed by the manifest, not re-derived. ──
        ["WC-GOV-053"] = new("daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision"),
        ["WC-GOV-054"] = new("daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision"),
        ["WC-GOV-055"] = new("daily_decision", "integration_governance", "concept_business_case", "construction_infrastructure", "single_decision"),
        ["WC-GOV-056"] = new("daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision"),
        ["WC-GOV-057"] = new("daily_decision", "integration_governance", "concept_business_case", "energy_utilities", "single_decision"),
        ["WC-GOV-058"] = new("daily_decision", "integration_governance", "concept_business_case", "technology_digital", "single_decision"),
        ["WC-GOV-059"] = new("daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision"),
        ["WC-GOV-060"] = new("daily_decision", "integration_governance", "concept_business_case", "construction_infrastructure", "single_decision"),
        ["WC-GOV-061"] = new("daily_decision", "integration_governance", "concept_business_case", "manufacturing_industrial", "single_decision"),
        ["WC-GOV-062"] = new("daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision"),
        ["WC-GOV-063"] = new("daily_decision", "integration_governance", "concept_business_case", "public_sector", "single_decision"),
        ["WC-STK-064"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "public_sector", "negotiation_communication"),
        ["WC-STK-065"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "transport_logistics", "negotiation_communication"),
        ["WC-STK-066"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "construction_infrastructure", "negotiation_communication"),
        ["WC-STK-067"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "technology_digital", "negotiation_communication"),
        ["WC-STK-068"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "cross_sector", "negotiation_communication"),
        ["WC-STK-069"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "cross_sector", "negotiation_communication"),
        ["WC-STK-070"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "energy_utilities", "negotiation_communication"),
        ["WC-STK-071"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "construction_infrastructure", "negotiation_communication"),
        ["WC-RSK-072"] = new("risk_room", "risk_uncertainty", "concept_business_case", "cross_sector", "numeric_calculation"),
        ["WC-RSK-073"] = new("risk_room", "risk_uncertainty", "concept_business_case", "energy_utilities", "numeric_calculation"),
        ["WC-RSK-074"] = new("risk_room", "risk_uncertainty", "concept_business_case", "healthcare_life_sciences", "numeric_calculation"),
        ["WC-RSK-075"] = new("risk_room", "risk_uncertainty", "concept_business_case", "climate_sustainability", "numeric_calculation"),
        ["WC-RSK-076"] = new("risk_room", "risk_uncertainty", "concept_business_case", "cross_sector", "numeric_calculation"),
        ["WC-RSK-077"] = new("risk_room", "risk_uncertainty", "concept_business_case", "technology_digital", "numeric_calculation"),
        ["WC-CPM-078"] = new("schedule_strategy", "schedule_planning", "concept_business_case", "construction_infrastructure", "numeric_calculation"),
        ["WC-CPM-079"] = new("schedule_strategy", "schedule_planning", "concept_business_case", "manufacturing_industrial", "numeric_calculation"),
        ["WC-EVM-080"] = new("cost_value", "cost_commercial", "concept_business_case", "cross_sector", "numeric_calculation"),

        // ── Year-1 February authored pack — scope, requirements and stakeholder alignment. ──
        ["WC-SCO-081"] = new("logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank"),
        ["WC-SCO-082"] = new("logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank"),
        ["WC-SCO-083"] = new("logic_sequence", "scope_requirements", "definition_planning", "construction_infrastructure", "order_rank"),
        ["WC-SCO-084"] = new("logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank"),
        ["WC-SCO-085"] = new("logic_sequence", "scope_requirements", "definition_planning", "energy_utilities", "order_rank"),
        ["WC-SCO-086"] = new("logic_sequence", "scope_requirements", "definition_planning", "technology_digital", "order_rank"),
        ["WC-SCO-087"] = new("logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank"),
        ["WC-SCO-088"] = new("logic_sequence", "scope_requirements", "definition_planning", "construction_infrastructure", "order_rank"),
        ["WC-SCO-089"] = new("logic_sequence", "scope_requirements", "definition_planning", "manufacturing_industrial", "order_rank"),
        ["WC-STK-090"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "cross_sector", "negotiation_communication"),
        ["WC-STK-091"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "public_sector", "negotiation_communication"),
        ["WC-STK-092"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "transport_logistics", "negotiation_communication"),
        ["WC-STK-093"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "cross_sector", "negotiation_communication"),
        ["WC-STK-094"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "construction_infrastructure", "negotiation_communication"),
        ["WC-STK-095"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "energy_utilities", "negotiation_communication"),
        ["WC-STK-096"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "healthcare_life_sciences", "negotiation_communication"),
        ["WC-STK-097"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "technology_digital", "negotiation_communication"),
        ["WC-STK-098"] = new("stakeholder_dilemma", "stakeholders_communication", "definition_planning", "cross_sector", "negotiation_communication"),
        ["WC-EVM-099"] = new("cost_value", "cost_commercial", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-CHG-100"] = new("cost_value", "cost_commercial", "definition_planning", "manufacturing_industrial", "numeric_calculation"),
        ["WC-CAP-101"] = new("executive_mission", "integration_governance", "definition_planning", "cross_sector", "multi_stage_decision"),
        ["WC-CAP-102"] = new("executive_mission", "integration_governance", "definition_planning", "technology_digital", "multi_stage_decision"),
        ["WC-RSK-103"] = new("risk_room", "risk_uncertainty", "definition_planning", "construction_infrastructure", "numeric_calculation"),
        ["WC-RSK-104"] = new("risk_room", "risk_uncertainty", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-CPM-105"] = new("schedule_strategy", "schedule_planning", "definition_planning", "climate_sustainability", "numeric_calculation"),
        ["WC-CPM-106"] = new("schedule_strategy", "schedule_planning", "definition_planning", "energy_utilities", "numeric_calculation"),

        // ── Year-1 March authored pack — planning, sequencing and critical-path judgment. ──
        ["WC-SCH-107"] = new("schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-SCH-108"] = new("schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-SCH-109"] = new("schedule_strategy", "schedule_planning", "definition_planning", "construction_infrastructure", "numeric_calculation"),
        ["WC-SCH-110"] = new("schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-SCH-111"] = new("schedule_strategy", "schedule_planning", "definition_planning", "energy_utilities", "numeric_calculation"),
        ["WC-SCH-112"] = new("schedule_strategy", "schedule_planning", "definition_planning", "technology_digital", "numeric_calculation"),
        ["WC-SCH-113"] = new("schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-SCH-114"] = new("schedule_strategy", "schedule_planning", "definition_planning", "construction_infrastructure", "numeric_calculation"),
        ["WC-SCH-115"] = new("schedule_strategy", "schedule_planning", "definition_planning", "manufacturing_industrial", "numeric_calculation"),
        ["WC-CBS-116"] = new("cost_value", "cost_commercial", "definition_planning", "public_sector", "numeric_calculation"),
        ["WC-CSH-117"] = new("cost_value", "cost_commercial", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-BOQ-118"] = new("cost_value", "cost_commercial", "definition_planning", "healthcare_life_sciences", "numeric_calculation"),
        ["WC-RSK-119"] = new("risk_room", "risk_uncertainty", "definition_planning", "cross_sector", "numeric_calculation"),
        ["WC-RSK-120"] = new("risk_room", "risk_uncertainty", "definition_planning", "construction_infrastructure", "numeric_calculation"),
        ["WC-RSK-121"] = new("risk_room", "risk_uncertainty", "definition_planning", "technology_digital", "evidence_diagnosis"),
        ["WC-CAP-122"] = new("executive_mission", "integration_governance", "definition_planning", "transport_logistics", "multi_stage_decision"),
        ["WC-CAP-123"] = new("executive_mission", "integration_governance", "definition_planning", "energy_utilities", "multi_stage_decision"),
        ["WC-STK-124"] = new("stakeholder_dilemma", "resources_leadership", "definition_planning", "cross_sector", "negotiation_communication"),

        // ── Year-1 April authored pack — cost, value, forecasting and commercial awareness. ──
        ["WC-CST-125"] = new("cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision"),
        ["WC-CST-126"] = new("cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision"),
        ["WC-CST-127"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-CST-128"] = new("cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision"),
        ["WC-CST-129"] = new("cost_value", "cost_commercial", "execution_control", "energy_utilities", "single_decision"),
        ["WC-CST-130"] = new("cost_value", "cost_commercial", "execution_control", "technology_digital", "single_decision"),
        ["WC-CST-131"] = new("cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision"),
        ["WC-CST-132"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-CST-133"] = new("cost_value", "cost_commercial", "execution_control", "manufacturing_industrial", "single_decision"),
        ["WC-CST-134"] = new("cost_value", "cost_commercial", "execution_control", "public_sector", "single_decision"),
        ["WC-QLT-135"] = new("daily_decision", "quality_assurance", "execution_control", "healthcare_life_sciences", "single_decision"),
        ["WC-PRC-136"] = new("daily_decision", "procurement_contracts", "execution_control", "climate_sustainability", "single_decision"),
        ["WC-GOV-137"] = new("daily_decision", "integration_governance", "execution_control", "cross_sector", "single_decision"),
        ["WC-QLT-138"] = new("daily_decision", "quality_assurance", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-PRC-139"] = new("daily_decision", "procurement_contracts", "execution_control", "energy_utilities", "single_decision"),
        ["WC-GOV-140"] = new("daily_decision", "integration_governance", "execution_control", "manufacturing_industrial", "single_decision"),
        ["WC-RSK-141"] = new("risk_room", "risk_uncertainty", "execution_control", "transport_logistics", "evidence_diagnosis"),
        ["WC-RSK-142"] = new("risk_room", "risk_uncertainty", "execution_control", "energy_utilities", "evidence_diagnosis"),
        ["WC-RSK-143"] = new("risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis"),
        ["WC-RSK-144"] = new("risk_room", "risk_uncertainty", "execution_control", "professional_services_other", "evidence_diagnosis"),
        ["WC-STK-145"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "negotiation_communication"),
        ["WC-STK-146"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "technology_digital", "negotiation_communication"),
        ["WC-STK-147"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "single_decision"),
        ["WC-CAP-148"] = new("executive_mission", "integration_governance", "execution_control", "cross_sector", "multi_stage_decision"),
        ["WC-CAP-149"] = new("executive_mission", "integration_governance", "execution_control", "construction_infrastructure", "multi_stage_decision"),

        // ── Year-1 May authored pack — risk, opportunity, uncertainty and contingency. ──
        ["WC-RSK-150"] = new("risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis"),
        ["WC-RSK-151"] = new("risk_room", "risk_uncertainty", "execution_control", "construction_infrastructure", "evidence_diagnosis"),
        ["WC-RSK-152"] = new("risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis"),
        ["WC-RSK-153"] = new("risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis"),
        ["WC-RSK-154"] = new("risk_room", "risk_uncertainty", "execution_control", "energy_utilities", "evidence_diagnosis"),
        ["WC-RSK-155"] = new("risk_room", "risk_uncertainty", "execution_control", "technology_digital", "evidence_diagnosis"),
        ["WC-RSK-156"] = new("risk_room", "risk_uncertainty", "execution_control", "construction_infrastructure", "evidence_diagnosis"),
        ["WC-RSK-157"] = new("risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis"),
        ["WC-RSK-158"] = new("risk_room", "risk_uncertainty", "execution_control", "public_sector", "evidence_diagnosis"),
        ["WC-RSK-159"] = new("risk_room", "risk_uncertainty", "execution_control", "manufacturing_industrial", "evidence_diagnosis"),
        ["WC-QLT-160"] = new("daily_decision", "quality_assurance", "execution_control", "transport_logistics", "single_decision"),
        ["WC-QLT-161"] = new("daily_decision", "quality_assurance", "execution_control", "healthcare_life_sciences", "single_decision"),
        ["WC-QLT-162"] = new("daily_decision", "quality_assurance", "execution_control", "cross_sector", "single_decision"),
        ["WC-PRC-163"] = new("daily_decision", "procurement_contracts", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-PRC-164"] = new("daily_decision", "procurement_contracts", "execution_control", "cross_sector", "single_decision"),
        ["WC-PRC-165"] = new("daily_decision", "procurement_contracts", "execution_control", "manufacturing_industrial", "single_decision"),
        ["WC-STK-166"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "single_decision"),
        ["WC-STK-167"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "technology_digital", "single_decision"),
        ["WC-STK-168"] = new("stakeholder_dilemma", "stakeholders_communication", "execution_control", "climate_sustainability", "single_decision"),
        ["WC-STK-169"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "public_sector", "single_decision"),
        ["WC-STK-170"] = new("stakeholder_dilemma", "stakeholders_communication", "execution_control", "energy_utilities", "single_decision"),
        ["WC-SCO-173"] = new("logic_sequence", "scope_requirements", "execution_control", "energy_utilities", "order_rank"),
        ["WC-SCO-174"] = new("logic_sequence", "scope_requirements", "execution_control", "cross_sector", "order_rank"),
        ["WC-CAP-171"] = new("executive_mission", "integration_governance", "execution_control", "cross_sector", "multi_stage_decision"),
        ["WC-CAP-172"] = new("executive_mission", "integration_governance", "execution_control", "construction_infrastructure", "multi_stage_decision"),

        // ── Year-1 June authored pack — procurement, contracts, claims and supplier decisions. ──
        ["WC-PRC-175"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-PRC-176"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "single_decision"),
        ["WC-PRC-177"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-PRC-178"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-PRC-179"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "energy_utilities", "single_decision"),
        ["WC-PRC-180"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "technology_digital", "single_decision"),
        ["WC-PRC-181"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "single_decision"),
        ["WC-PRC-182"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-PRC-183"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "public_sector", "single_decision"),
        ["WC-PRC-184"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "transport_logistics", "single_decision"),
        ["WC-QLT-185"] = new("daily_decision", "quality_assurance", "procurement_mobilization", "healthcare_life_sciences", "single_decision"),
        ["WC-QLT-186"] = new("daily_decision", "quality_assurance", "procurement_mobilization", "construction_infrastructure", "single_decision"),
        ["WC-QLT-187"] = new("daily_decision", "quality_assurance", "procurement_mobilization", "energy_utilities", "single_decision"),
        ["WC-GOV-188"] = new("daily_decision", "integration_governance", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-SCO-189"] = new("logic_sequence", "scope_requirements", "procurement_mobilization", "cross_sector", "order_rank"),
        ["WC-SCO-190"] = new("logic_sequence", "scope_requirements", "procurement_mobilization", "climate_sustainability", "order_rank"),
        ["WC-SCO-191"] = new("logic_sequence", "scope_requirements", "procurement_mobilization", "public_sector", "order_rank"),
        ["WC-SCH-192"] = new("schedule_strategy", "schedule_planning", "procurement_mobilization", "construction_infrastructure", "order_rank"),
        ["WC-SCH-193"] = new("schedule_strategy", "schedule_planning", "procurement_mobilization", "cross_sector", "order_rank"),
        ["WC-SCH-194"] = new("schedule_strategy", "schedule_planning", "procurement_mobilization", "transport_logistics", "order_rank"),
        ["WC-STK-195"] = new("stakeholder_dilemma", "resources_leadership", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-STK-196"] = new("stakeholder_dilemma", "stakeholders_communication", "procurement_mobilization", "energy_utilities", "single_decision"),
        ["WC-STK-197"] = new("stakeholder_dilemma", "resources_leadership", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-STK-198"] = new("stakeholder_dilemma", "stakeholders_communication", "procurement_mobilization", "professional_services_other", "single_decision"),
        ["WC-CAP-199"] = new("executive_mission", "integration_governance", "procurement_mobilization", "manufacturing_industrial", "multi_stage_decision"),
        ["WC-CAP-200"] = new("executive_mission", "integration_governance", "procurement_mobilization", "technology_digital", "multi_stage_decision"),

        // ── Year-1 July authored pack — delivery control, change and recovery. ──
        ["WC-RSC-201"] = new("project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-202"] = new("project_rescue", "change_claims_recovery", "execution_control", "construction_infrastructure", "multi_stage_decision"),
        ["WC-RSC-203"] = new("project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-204"] = new("project_rescue", "change_claims_recovery", "execution_control", "technology_digital", "multi_stage_decision"),
        ["WC-RSC-205"] = new("project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-206"] = new("project_rescue", "change_claims_recovery", "execution_control", "energy_utilities", "multi_stage_decision"),
        ["WC-RSC-207"] = new("project_rescue", "change_claims_recovery", "execution_control", "construction_infrastructure", "multi_stage_decision"),
        ["WC-RSC-208"] = new("project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-209"] = new("project_rescue", "change_claims_recovery", "execution_control", "manufacturing_industrial", "multi_stage_decision"),
        ["WC-RSC-210"] = new("project_rescue", "change_claims_recovery", "execution_control", "public_sector", "multi_stage_decision"),
        ["WC-CST-211"] = new("cost_value", "cost_commercial", "execution_control", "transport_logistics", "single_decision"),
        ["WC-CST-212"] = new("cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision"),
        ["WC-CST-213"] = new("cost_value", "cost_commercial", "execution_control", "healthcare_life_sciences", "single_decision"),
        ["WC-CST-214"] = new("cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-CST-215"] = new("cost_value", "cost_commercial", "execution_control", "technology_digital", "single_decision"),
        ["WC-CST-216"] = new("cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision"),
        ["WC-CST-217"] = new("cost_value", "cost_commercial", "execution_control", "climate_sustainability", "single_decision"),
        ["WC-CST-218"] = new("cost_value", "cost_commercial", "execution_control", "energy_utilities", "single_decision"),
        ["WC-GOV-219"] = new("daily_decision", "integration_governance", "execution_control", "cross_sector", "single_decision"),
        ["WC-QLT-220"] = new("daily_decision", "quality_assurance", "execution_control", "cross_sector", "single_decision"),
        ["WC-COM-221"] = new("daily_decision", "stakeholders_communication", "execution_control", "public_sector", "single_decision"),
        ["WC-STK-222"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "single_decision"),
        ["WC-STK-223"] = new("stakeholder_dilemma", "stakeholders_communication", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-STK-224"] = new("stakeholder_dilemma", "resources_leadership", "execution_control", "manufacturing_industrial", "single_decision"),
        ["WC-SCO-225"] = new("logic_sequence", "scope_requirements", "execution_control", "professional_services_other", "order_rank"),
        ["WC-SCH-226"] = new("schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "order_rank"),

        // ── Year-1 August authored pack — leadership, teams, communication and conflict. ──
        ["WC-COM-227"] = new("daily_decision", "stakeholders_communication", "execution_control", "public_sector", "single_decision"),
        ["WC-COM-228"] = new("daily_decision", "stakeholders_communication", "execution_control", "cross_sector", "single_decision"),
        ["WC-COM-229"] = new("daily_decision", "stakeholders_communication", "execution_control", "healthcare_life_sciences", "single_decision"),
        ["WC-COM-230"] = new("daily_decision", "stakeholders_communication", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-COM-231"] = new("daily_decision", "stakeholders_communication", "execution_control", "climate_sustainability", "single_decision"),
        ["WC-COM-232"] = new("daily_decision", "stakeholders_communication", "execution_control", "energy_utilities", "single_decision"),
        ["WC-COM-233"] = new("daily_decision", "stakeholders_communication", "execution_control", "technology_digital", "single_decision"),
        ["WC-COM-234"] = new("daily_decision", "stakeholders_communication", "execution_control", "cross_sector", "single_decision"),
        ["WC-COM-235"] = new("daily_decision", "stakeholders_communication", "execution_control", "cross_sector", "single_decision"),
        ["WC-COM-236"] = new("daily_decision", "stakeholders_communication", "execution_control", "construction_infrastructure", "single_decision"),
        ["WC-RES-237"] = new("schedule_strategy", "resources_leadership", "execution_control", "cross_sector", "order_rank"),
        ["WC-RES-238"] = new("schedule_strategy", "resources_leadership", "execution_control", "construction_infrastructure", "order_rank"),
        ["WC-RES-239"] = new("schedule_strategy", "resources_leadership", "execution_control", "cross_sector", "order_rank"),
        ["WC-RES-240"] = new("schedule_strategy", "resources_leadership", "execution_control", "energy_utilities", "order_rank"),
        ["WC-RES-241"] = new("schedule_strategy", "resources_leadership", "execution_control", "technology_digital", "order_rank"),
        ["WC-RES-242"] = new("schedule_strategy", "resources_leadership", "execution_control", "cross_sector", "order_rank"),
        ["WC-RES-243"] = new("schedule_strategy", "resources_leadership", "execution_control", "construction_infrastructure", "order_rank"),
        ["WC-RES-244"] = new("schedule_strategy", "resources_leadership", "execution_control", "cross_sector", "order_rank"),
        ["WC-RES-245"] = new("schedule_strategy", "resources_leadership", "execution_control", "transport_logistics", "order_rank"),
        ["WC-RES-246"] = new("schedule_strategy", "resources_leadership", "execution_control", "manufacturing_industrial", "order_rank"),
        ["WC-SCH-247"] = new("schedule_strategy", "schedule_planning", "execution_control", "transport_logistics", "order_rank"),
        ["WC-GOV-248"] = new("daily_decision", "integration_governance", "execution_control", "cross_sector", "single_decision"),
        ["WC-GOV-249"] = new("daily_decision", "integration_governance", "execution_control", "technology_digital", "single_decision"),
        ["WC-GOV-250"] = new("daily_decision", "integration_governance", "definition_planning", "construction_infrastructure", "single_decision"),
        ["WC-QLT-251"] = new("daily_decision", "quality_assurance", "execution_control", "energy_utilities", "single_decision"),
        ["WC-QLT-252"] = new("daily_decision", "quality_assurance", "definition_planning", "public_sector", "single_decision"),
        ["WC-SCO-253"] = new("logic_sequence", "scope_requirements", "execution_control", "professional_services_other", "order_rank"),
        ["WC-SCO-254"] = new("logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank"),
        ["WC-SCH-255"] = new("logic_sequence", "schedule_planning", "definition_planning", "manufacturing_industrial", "order_rank"),
        // September authored pack — commissioning, handover and closeout quality.
        ["WC-QLT-256"] = new("daily_decision", "quality_assurance", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-QLT-257"] = new("daily_decision", "quality_assurance", "commissioning_handover", "construction_infrastructure", "single_decision"),
        ["WC-QLT-258"] = new("daily_decision", "quality_assurance", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-QLT-259"] = new("daily_decision", "quality_assurance", "commissioning_handover", "energy_utilities", "single_decision"),
        ["WC-QLT-260"] = new("daily_decision", "quality_assurance", "commissioning_handover", "technology_digital", "single_decision"),
        ["WC-QLT-261"] = new("daily_decision", "quality_assurance", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-QLT-262"] = new("daily_decision", "quality_assurance", "commissioning_handover", "construction_infrastructure", "single_decision"),
        ["WC-QLT-263"] = new("daily_decision", "quality_assurance", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-QLT-264"] = new("daily_decision", "quality_assurance", "commissioning_handover", "transport_logistics", "single_decision"),
        ["WC-QLT-265"] = new("daily_decision", "quality_assurance", "commissioning_handover", "healthcare_life_sciences", "single_decision"),
        ["WC-SAF-266"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "manufacturing_industrial", "single_decision"),
        ["WC-SAF-267"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "public_sector", "single_decision"),
        ["WC-SAF-268"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-SAF-269"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "climate_sustainability", "single_decision"),
        ["WC-SAF-270"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "construction_infrastructure", "single_decision"),
        ["WC-SAF-271"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "energy_utilities", "single_decision"),
        ["WC-SAF-272"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "technology_digital", "single_decision"),
        ["WC-SAF-273"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-SAF-274"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-SAF-275"] = new("daily_decision", "safety_sustainability", "commissioning_handover", "construction_infrastructure", "single_decision"),
        ["WC-GOV-276"] = new("daily_decision", "integration_governance", "commissioning_handover", "transport_logistics", "single_decision"),
        ["WC-GOV-277"] = new("daily_decision", "integration_governance", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-SCO-278"] = new("logic_sequence", "scope_requirements", "commissioning_handover", "cross_sector", "order_rank"),
        ["WC-SCO-279"] = new("logic_sequence", "scope_requirements", "commissioning_handover", "energy_utilities", "order_rank"),
        ["WC-SCO-280"] = new("logic_sequence", "scope_requirements", "commissioning_handover", "healthcare_life_sciences", "order_rank"),
        ["WC-SCH-281"] = new("logic_sequence", "schedule_planning", "commissioning_handover", "professional_services_other", "order_rank"),
        ["WC-SCH-282"] = new("logic_sequence", "schedule_planning", "commissioning_handover", "technology_digital", "order_rank"),
        ["WC-SCH-283"] = new("logic_sequence", "schedule_planning", "commissioning_handover", "manufacturing_industrial", "order_rank"),
        // October authored pack — digital delivery, data quality and responsible AI (all advanced).
        ["WC-AIA-284"] = new("daily_decision", "digital_data_ai", "definition_planning", "cross_sector", "single_decision"),
        ["WC-DQA-285"] = new("daily_decision", "digital_data_ai", "definition_planning", "construction_infrastructure", "single_decision"),
        ["WC-AIA-286"] = new("daily_decision", "digital_data_ai", "definition_planning", "cross_sector", "single_decision"),
        ["WC-DQA-287"] = new("daily_decision", "digital_data_ai", "definition_planning", "energy_utilities", "single_decision"),
        ["WC-AIA-288"] = new("daily_decision", "digital_data_ai", "definition_planning", "technology_digital", "single_decision"),
        ["WC-RSC-289"] = new("project_rescue", "schedule_planning", "definition_planning", "healthcare_life_sciences", "multi_stage_decision"),
        ["WC-RSC-290"] = new("project_rescue", "schedule_planning", "definition_planning", "energy_utilities", "multi_stage_decision"),
        ["WC-RSC-291"] = new("project_rescue", "schedule_planning", "definition_planning", "professional_services_other", "multi_stage_decision"),
        ["WC-RSC-292"] = new("project_rescue", "schedule_planning", "closeout_lessons", "technology_digital", "multi_stage_decision"),
        ["WC-RSK-293"] = new("risk_room", "risk_uncertainty", "definition_planning", "transport_logistics", "evidence_diagnosis"),
        ["WC-RSK-294"] = new("risk_room", "risk_uncertainty", "definition_planning", "manufacturing_industrial", "evidence_diagnosis"),
        ["WC-RSK-295"] = new("risk_room", "risk_uncertainty", "definition_planning", "technology_digital", "evidence_diagnosis"),
        ["WC-RSK-296"] = new("risk_room", "risk_uncertainty", "definition_planning", "public_sector", "evidence_diagnosis"),
        ["WC-QLT-297"] = new("daily_decision", "quality_assurance", "definition_planning", "cross_sector", "single_decision"),
        ["WC-QLT-298"] = new("daily_decision", "quality_assurance", "definition_planning", "construction_infrastructure", "single_decision"),
        ["WC-QLT-299"] = new("daily_decision", "quality_assurance", "definition_planning", "public_sector", "single_decision"),
        ["WC-GOV-300"] = new("daily_decision", "integration_governance", "definition_planning", "cross_sector", "single_decision"),
        ["WC-GOV-301"] = new("daily_decision", "integration_governance", "definition_planning", "cross_sector", "single_decision"),
        ["WC-GOV-302"] = new("daily_decision", "integration_governance", "definition_planning", "cross_sector", "single_decision"),
        ["WC-GOV-303"] = new("daily_decision", "integration_governance", "closeout_lessons", "cross_sector", "single_decision"),
        ["WC-SCO-304"] = new("daily_decision", "scope_requirements", "definition_planning", "climate_sustainability", "single_decision"),
        ["WC-SCO-305"] = new("daily_decision", "scope_requirements", "definition_planning", "construction_infrastructure", "single_decision"),
        ["WC-SCO-306"] = new("daily_decision", "scope_requirements", "definition_planning", "construction_infrastructure", "single_decision"),
        ["WC-SCO-307"] = new("daily_decision", "scope_requirements", "closeout_lessons", "energy_utilities", "single_decision"),
        ["WC-PRC-308"] = new("daily_decision", "procurement_contracts", "definition_planning", "cross_sector", "single_decision"),
        ["WC-PRC-309"] = new("daily_decision", "procurement_contracts", "definition_planning", "transport_logistics", "single_decision"),
        ["WC-CST-310"] = new("daily_decision", "cost_commercial", "closeout_lessons", "construction_infrastructure", "single_decision"),
        // November authored pack — integrated project controls and executive trade-offs (all advanced, deep).
        ["WC-RSC-311"] = new("project_rescue", "cost_commercial", "closeout_lessons", "cross_sector", "multi_stage_decision"),
        ["WC-GOV-312"] = new("daily_decision", "integration_governance", "closeout_lessons", "cross_sector", "single_decision"),
        ["WC-RSK-313"] = new("risk_room", "risk_uncertainty", "concept_business_case", "construction_infrastructure", "evidence_diagnosis"),
        ["WC-RSC-314"] = new("project_rescue", "procurement_contracts", "procurement_mobilization", "cross_sector", "multi_stage_decision"),
        ["WC-GOV-315"] = new("daily_decision", "integration_governance", "closeout_lessons", "climate_sustainability", "single_decision"),
        ["WC-RSK-316"] = new("risk_room", "risk_uncertainty", "closeout_lessons", "cross_sector", "evidence_diagnosis"),
        ["WC-RSC-317"] = new("project_rescue", "change_claims_recovery", "closeout_lessons", "climate_sustainability", "multi_stage_decision"),
        ["WC-GOV-318"] = new("daily_decision", "integration_governance", "closeout_lessons", "healthcare_life_sciences", "single_decision"),
        ["WC-RSK-319"] = new("risk_room", "risk_uncertainty", "procurement_mobilization", "professional_services_other", "evidence_diagnosis"),
        ["WC-RSC-320"] = new("project_rescue", "scope_requirements", "concept_business_case", "healthcare_life_sciences", "multi_stage_decision"),
        ["WC-GOV-321"] = new("daily_decision", "integration_governance", "closeout_lessons", "manufacturing_industrial", "single_decision"),
        ["WC-RSC-322"] = new("project_rescue", "schedule_planning", "procurement_mobilization", "manufacturing_industrial", "multi_stage_decision"),
        ["WC-GOV-323"] = new("daily_decision", "integration_governance", "closeout_lessons", "cross_sector", "single_decision"),
        ["WC-RSC-324"] = new("project_rescue", "procurement_contracts", "concept_business_case", "construction_infrastructure", "multi_stage_decision"),
        ["WC-GOV-325"] = new("daily_decision", "integration_governance", "closeout_lessons", "construction_infrastructure", "single_decision"),
        ["WC-RSC-326"] = new("project_rescue", "change_claims_recovery", "procurement_mobilization", "energy_utilities", "multi_stage_decision"),
        ["WC-CST-327"] = new("daily_decision", "cost_commercial", "procurement_mobilization", "cross_sector", "evidence_diagnosis"),
        ["WC-RSC-328"] = new("project_rescue", "scope_requirements", "closeout_lessons", "technology_digital", "multi_stage_decision"),
        ["WC-CST-329"] = new("daily_decision", "cost_commercial", "closeout_lessons", "energy_utilities", "evidence_diagnosis"),
        ["WC-RSC-330"] = new("project_rescue", "resources_leadership", "closeout_lessons", "public_sector", "multi_stage_decision"),
        ["WC-CST-331"] = new("daily_decision", "cost_commercial", "procurement_mobilization", "public_sector", "evidence_diagnosis"),
        ["WC-RSC-332"] = new("project_rescue", "stakeholders_communication", "concept_business_case", "transport_logistics", "multi_stage_decision"),
        ["WC-CST-333"] = new("daily_decision", "cost_commercial", "closeout_lessons", "technology_digital", "evidence_diagnosis"),
        ["WC-RSC-334"] = new("project_rescue", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "multi_stage_decision"),
        ["WC-CST-335"] = new("daily_decision", "cost_commercial", "procurement_mobilization", "transport_logistics", "evidence_diagnosis"),
        ["WC-RSC-336"] = new("project_rescue", "schedule_planning", "concept_business_case", "cross_sector", "multi_stage_decision"),
        // December authored pack — handover, closeout, lessons and capstone missions.
        ["WC-RSC-337"] = new("project_rescue", "stakeholders_communication", "closeout_lessons", "cross_sector", "multi_stage_decision"),
        ["WC-RSK-338"] = new("risk_room", "risk_uncertainty", "closeout_lessons", "healthcare_life_sciences", "evidence_diagnosis"),
        ["WC-RSC-339"] = new("project_rescue", "stakeholders_communication", "closeout_lessons", "cross_sector", "multi_stage_decision"),
        ["WC-RSK-340"] = new("risk_room", "risk_uncertainty", "concept_business_case", "climate_sustainability", "evidence_diagnosis"),
        ["WC-RSC-341"] = new("project_rescue", "stakeholders_communication", "closeout_lessons", "energy_utilities", "multi_stage_decision"),
        ["WC-RSK-342"] = new("risk_room", "risk_uncertainty", "concept_business_case", "professional_services_other", "order_rank"),
        ["WC-RSC-343"] = new("project_rescue", "stakeholders_communication", "closeout_lessons", "technology_digital", "multi_stage_decision"),
        ["WC-RSK-344"] = new("risk_room", "change_claims_recovery", "procurement_mobilization", "transport_logistics", "order_rank"),
        ["WC-RSC-345"] = new("project_rescue", "change_claims_recovery", "closeout_lessons", "construction_infrastructure", "multi_stage_decision"),
        ["WC-RSK-346"] = new("risk_room", "risk_uncertainty", "commissioning_handover", "cross_sector", "evidence_diagnosis"),
        ["WC-RSC-347"] = new("project_rescue", "scope_requirements", "closeout_lessons", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-348"] = new("project_rescue", "schedule_planning", "closeout_lessons", "climate_sustainability", "multi_stage_decision"),
        ["WC-RSC-349"] = new("project_rescue", "resources_leadership", "commissioning_handover", "manufacturing_industrial", "multi_stage_decision"),
        ["WC-RSC-350"] = new("project_rescue", "procurement_contracts", "commissioning_handover", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-351"] = new("project_rescue", "change_claims_recovery", "commissioning_handover", "public_sector", "multi_stage_decision"),
        ["WC-RSC-352"] = new("project_rescue", "scope_requirements", "commissioning_handover", "transport_logistics", "multi_stage_decision"),
        ["WC-RSC-353"] = new("project_rescue", "schedule_planning", "commissioning_handover", "construction_infrastructure", "multi_stage_decision"),
        ["WC-RSC-354"] = new("project_rescue", "resources_leadership", "commissioning_handover", "energy_utilities", "multi_stage_decision"),
        ["WC-RSC-355"] = new("project_rescue", "procurement_contracts", "concept_business_case", "technology_digital", "multi_stage_decision"),
        ["WC-RSC-356"] = new("project_rescue", "change_claims_recovery", "procurement_mobilization", "cross_sector", "multi_stage_decision"),
        ["WC-RSC-357"] = new("project_rescue", "scope_requirements", "concept_business_case", "construction_infrastructure", "multi_stage_decision"),
        ["WC-RSC-358"] = new("project_rescue", "schedule_planning", "procurement_mobilization", "cross_sector", "evidence_diagnosis"),
        ["WC-RSC-359"] = new("project_rescue", "resources_leadership", "procurement_mobilization", "construction_infrastructure", "evidence_diagnosis"),
        ["WC-RSC-360"] = new("project_rescue", "procurement_contracts", "concept_business_case", "cross_sector", "evidence_diagnosis"),
        ["WC-RSC-361"] = new("project_rescue", "change_claims_recovery", "procurement_mobilization", "energy_utilities", "evidence_diagnosis"),
        ["WC-RSC-362"] = new("project_rescue", "scope_requirements", "concept_business_case", "healthcare_life_sciences", "order_rank"),
        ["WC-RSC-363"] = new("project_rescue", "schedule_planning", "procurement_mobilization", "manufacturing_industrial", "order_rank"),
        ["WC-RSC-364"] = new("project_rescue", "resources_leadership", "procurement_mobilization", "public_sector", "order_rank"),
        ["WC-RSC-365"] = new("project_rescue", "procurement_contracts", "concept_business_case", "technology_digital", "order_rank"),
        // Reserve bank — PI-Y1-R001..R055 substitution stock.
        ["WC-CHG-366"] = new("daily_decision", "change_claims_recovery", "definition_planning", "construction_infrastructure", "single_decision"),
        ["WC-PRC-367"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "energy_utilities", "single_decision"),
        ["WC-QLT-368"] = new("daily_decision", "quality_assurance", "execution_control", "technology_digital", "single_decision"),
        ["WC-AIA-369"] = new("daily_decision", "digital_data_ai", "commissioning_handover", "manufacturing_industrial", "single_decision"),
        ["WC-SAF-370"] = new("daily_decision", "safety_sustainability", "closeout_lessons", "transport_logistics", "single_decision"),
        ["WC-GOV-371"] = new("daily_decision", "integration_governance", "concept_business_case", "public_sector", "single_decision"),
        ["WC-CHG-372"] = new("daily_decision", "change_claims_recovery", "definition_planning", "healthcare_life_sciences", "single_decision"),
        ["WC-PRC-373"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "climate_sustainability", "single_decision"),
        ["WC-QLT-374"] = new("daily_decision", "quality_assurance", "execution_control", "professional_services_other", "single_decision"),
        ["WC-DQA-375"] = new("daily_decision", "digital_data_ai", "commissioning_handover", "cross_sector", "single_decision"),
        ["WC-SAF-376"] = new("daily_decision", "safety_sustainability", "closeout_lessons", "construction_infrastructure", "single_decision"),
        ["WC-GOV-377"] = new("daily_decision", "integration_governance", "concept_business_case", "energy_utilities", "single_decision"),
        ["WC-CHG-378"] = new("daily_decision", "change_claims_recovery", "definition_planning", "technology_digital", "single_decision"),
        ["WC-PRC-379"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "manufacturing_industrial", "single_decision"),
        ["WC-QLT-380"] = new("daily_decision", "quality_assurance", "execution_control", "transport_logistics", "single_decision"),
        ["WC-AIA-381"] = new("daily_decision", "digital_data_ai", "commissioning_handover", "public_sector", "single_decision"),
        ["WC-SAF-382"] = new("daily_decision", "safety_sustainability", "closeout_lessons", "healthcare_life_sciences", "single_decision"),
        ["WC-GOV-383"] = new("daily_decision", "integration_governance", "concept_business_case", "climate_sustainability", "single_decision"),
        ["WC-CHG-384"] = new("daily_decision", "change_claims_recovery", "definition_planning", "professional_services_other", "single_decision"),
        ["WC-PRC-385"] = new("daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision"),
        ["WC-RSC-386"] = new("project_rescue", "schedule_planning", "execution_control", "construction_infrastructure", "multi_stage_decision"),
        ["WC-RSC-387"] = new("project_rescue", "cost_commercial", "commissioning_handover", "energy_utilities", "multi_stage_decision"),
        ["WC-RSC-388"] = new("project_rescue", "risk_uncertainty", "closeout_lessons", "technology_digital", "multi_stage_decision"),
        ["WC-RSC-389"] = new("project_rescue", "change_claims_recovery", "concept_business_case", "manufacturing_industrial", "multi_stage_decision"),
        ["WC-RSC-390"] = new("project_rescue", "schedule_planning", "definition_planning", "transport_logistics", "multi_stage_decision"),
        ["WC-RSC-391"] = new("project_rescue", "cost_commercial", "procurement_mobilization", "public_sector", "multi_stage_decision"),
        ["WC-RSC-392"] = new("project_rescue", "risk_uncertainty", "execution_control", "healthcare_life_sciences", "multi_stage_decision"),
        ["WC-RSC-393"] = new("project_rescue", "change_claims_recovery", "commissioning_handover", "climate_sustainability", "multi_stage_decision"),
        ["WC-RSK-394"] = new("risk_room", "risk_uncertainty", "closeout_lessons", "professional_services_other", "numeric_calculation"),
        ["WC-RSK-395"] = new("risk_room", "risk_uncertainty", "concept_business_case", "cross_sector", "numeric_calculation"),
        ["WC-RSK-396"] = new("risk_room", "risk_uncertainty", "definition_planning", "construction_infrastructure", "numeric_calculation"),
        ["WC-RSK-397"] = new("risk_room", "risk_uncertainty", "procurement_mobilization", "energy_utilities", "numeric_calculation"),
        ["WC-RSK-398"] = new("risk_room", "risk_uncertainty", "execution_control", "technology_digital", "numeric_calculation"),
        ["WC-RSK-399"] = new("risk_room", "risk_uncertainty", "commissioning_handover", "manufacturing_industrial", "numeric_calculation"),
        ["WC-RES-400"] = new("schedule_strategy", "resources_leadership", "closeout_lessons", "transport_logistics", "numeric_calculation"),
        ["WC-SCH-401"] = new("schedule_strategy", "schedule_planning", "concept_business_case", "public_sector", "numeric_calculation"),
        ["WC-RES-402"] = new("schedule_strategy", "resources_leadership", "definition_planning", "healthcare_life_sciences", "numeric_calculation"),
        ["WC-SCH-403"] = new("schedule_strategy", "schedule_planning", "procurement_mobilization", "climate_sustainability", "numeric_calculation"),
        ["WC-RES-404"] = new("schedule_strategy", "resources_leadership", "execution_control", "professional_services_other", "numeric_calculation"),
        ["WC-SCH-405"] = new("schedule_strategy", "schedule_planning", "commissioning_handover", "cross_sector", "numeric_calculation"),
        ["WC-CHG-406"] = new("cost_value", "procurement_contracts", "closeout_lessons", "construction_infrastructure", "numeric_calculation"),
        ["WC-CST-407"] = new("cost_value", "cost_commercial", "concept_business_case", "energy_utilities", "numeric_calculation"),
        ["WC-PRC-408"] = new("cost_value", "procurement_contracts", "definition_planning", "technology_digital", "numeric_calculation"),
        ["WC-CST-409"] = new("cost_value", "cost_commercial", "procurement_mobilization", "manufacturing_industrial", "numeric_calculation"),
        ["WC-PRC-410"] = new("cost_value", "procurement_contracts", "execution_control", "transport_logistics", "numeric_calculation"),
        ["WC-STK-411"] = new("stakeholder_dilemma", "resources_leadership", "commissioning_handover", "public_sector", "negotiation_communication"),
        ["WC-STK-412"] = new("stakeholder_dilemma", "integration_governance", "closeout_lessons", "healthcare_life_sciences", "negotiation_communication"),
        ["WC-STK-413"] = new("stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "climate_sustainability", "negotiation_communication"),
        ["WC-STK-414"] = new("stakeholder_dilemma", "resources_leadership", "definition_planning", "professional_services_other", "negotiation_communication"),
        ["WC-QLT-415"] = new("logic_sequence", "quality_assurance", "procurement_mobilization", "cross_sector", "order_rank"),
        ["WC-SCO-416"] = new("logic_sequence", "scope_requirements", "execution_control", "construction_infrastructure", "order_rank"),
        ["WC-SCH-417"] = new("logic_sequence", "schedule_planning", "commissioning_handover", "energy_utilities", "order_rank"),
        ["WC-QLT-418"] = new("logic_sequence", "quality_assurance", "closeout_lessons", "technology_digital", "order_rank"),
        ["WC-CAP-419"] = new("executive_mission", "integration_governance", "concept_business_case", "manufacturing_industrial", "multi_stage_decision"),
        ["WC-CAP-420"] = new("executive_mission", "integration_governance", "definition_planning", "transport_logistics", "multi_stage_decision"),
    };

    /// <summary>
    /// Idempotent taxonomy backfill for house challenges. Writes only pciworld_challenges.pi_*
    /// (catalogue metadata on the working copy) — never config_json and never a version snapshot,
    /// so replay immutability is untouched. Operator-authored rows (author_id set) are never
    /// touched; an operator can still re-classify any row through the admin surface later.
    /// </summary>
    public static void Backfill(Data.Db db)
    {
        foreach (var (code, m) in Classification)
        {
            db.Execute(@"UPDATE pciworld_challenges
                    SET pi_type=?, pi_domain=?, pi_lifecycle=?, pi_sector=?, pi_interaction=?
                WHERE code=? AND author_id IS NULL
                  AND (pi_type IS NULL OR pi_type<>? OR pi_domain IS NULL OR pi_domain<>?
                       OR pi_lifecycle IS NULL OR pi_lifecycle<>? OR pi_sector IS NULL OR pi_sector<>?
                       OR pi_interaction IS NULL OR pi_interaction<>?)",
                m.Type, m.Domain, m.Lifecycle, m.Sector, m.Interaction,
                code, m.Type, m.Domain, m.Lifecycle, m.Sector, m.Interaction);
        }
    }

    // ── Year-1 governed plan (backend/content/project-intelligence/year-1/). The plan is data the
    //    build verifies, not code: 365 scheduled slots + 55 reserve slots, each carrying its full
    //    taxonomy. `mapped` slots point at an existing bank challenge; `planned` slots are the
    //    authoring backlog the coverage report and the runway alert count against. ──

    public sealed record PlanEntry(
        string Code, int Day, string Status, string? SourceChallenge, string Title,
        string Type, string Domain, string DifficultyBand, string DurationBand, int EstMinutes,
        string Interaction, string Lifecycle, string Sector);

    public sealed record YearPlan(List<PlanEntry> Scheduled, List<PlanEntry> Reserve);

    /// <summary>Locate the year-1 content directory: the publish output first (content/ ships with
    /// the app), then repository ancestors (tests, dev runs).</summary>
    public static string? ContentRoot()
    {
        var candidates = new List<string> { AppContext.BaseDirectory, Directory.GetCurrentDirectory() };
        foreach (var start in candidates)
        {
            var dir = new DirectoryInfo(start);
            while (dir is not null)
            {
                var p = Path.Combine(dir.FullName, "content", "project-intelligence", "year-1");
                if (File.Exists(Path.Combine(p, "manifest.json"))) return p;
                var pb = Path.Combine(dir.FullName, "backend", "content", "project-intelligence", "year-1");
                if (File.Exists(Path.Combine(pb, "manifest.json"))) return pb;
                dir = dir.Parent;
            }
        }
        return null;
    }

    static PlanEntry Entry(JsonElement e) => new(
        e.GetProperty("code").GetString()!,
        e.TryGetProperty("day", out var d) && d.ValueKind == JsonValueKind.Number ? d.GetInt32() : 0,
        e.GetProperty("status").GetString()!,
        e.TryGetProperty("source_challenge", out var s) && s.ValueKind == JsonValueKind.String ? s.GetString() : null,
        e.GetProperty("title").GetString()!,
        e.GetProperty("experience_type").GetString()!,
        e.GetProperty("primary_domain").GetString()!,
        e.GetProperty("difficulty_band").GetString()!,
        e.GetProperty("duration_band").GetString()!,
        e.TryGetProperty("est_minutes", out var m) && m.ValueKind == JsonValueKind.Number ? m.GetInt32() : 0,
        e.GetProperty("interaction").GetString()!,
        e.GetProperty("lifecycle_stage").GetString()!,
        e.GetProperty("sector").GetString()!);

    /// <summary>Load and parse the whole Year-1 plan; null when the content directory is absent
    /// (the programme degrades to the live bank — it never breaks serving).</summary>
    public static YearPlan? LoadYearOne()
    {
        var root = ContentRoot();
        if (root is null) return null;
        try
        {
            var scheduled = new List<PlanEntry>();
            var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "manifest.json"))).RootElement;
            foreach (var f in manifest.GetProperty("month_files").EnumerateArray())
            {
                var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, f.GetString()!))).RootElement;
                foreach (var e in doc.GetProperty("entries").EnumerateArray()) scheduled.Add(Entry(e));
            }
            var reserve = new List<PlanEntry>();
            var rdoc = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "reserve.json"))).RootElement;
            foreach (var e in rdoc.GetProperty("entries").EnumerateArray()) reserve.Add(Entry(e));
            return new YearPlan(scheduled, reserve);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"[pci-intelligence] year-1 plan unreadable: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Coverage summary for the admin dashboard and the runway alert: how much of the Year-1 plan
    /// is backed by a published, servable bank challenge. Runway is counted from day 1 in plan
    /// order up to the first unbacked day — a plan with holes is a plan, not coverage.
    /// </summary>
    public static object Coverage(Data.Db db, YearPlan plan)
    {
        var servable = db.Query("SELECT code FROM pciworld_challenges WHERE current_version>=1 AND retired=0")
            .Select(r => H.Str(r["code"]) ?? "").ToHashSet(StringComparer.Ordinal);

        var byDay = plan.Scheduled.OrderBy(e => e.Day).ToList();
        var runway = 0;
        foreach (var e in byDay)
        {
            if (e.Status == "mapped" && e.SourceChallenge is not null && servable.Contains(e.SourceChallenge)) runway++;
            else break;
        }
        int backed = byDay.Count(e => e.Status == "mapped" && e.SourceChallenge is not null && servable.Contains(e.SourceChallenge));

        Dictionary<string, int> Tally(IEnumerable<PlanEntry> src, Func<PlanEntry, string> key) =>
            src.GroupBy(key).ToDictionary(g => g.Key, g => g.Count());

        return new
        {
            scheduled_total = byDay.Count,
            reserve_total = plan.Reserve.Count,
            bank_total = byDay.Count + plan.Reserve.Count,
            mapped = byDay.Count(e => e.Status == "mapped"),
            planned = byDay.Count(e => e.Status == "planned"),
            backed_by_published_challenge = backed,
            runway_days = runway,
            runway_alert = runway < 60,
            by_type = Tally(byDay, e => e.Type),
            by_domain = Tally(byDay, e => e.Domain),
            by_difficulty_band = Tally(byDay, e => e.DifficultyBand),
            by_duration_band = Tally(byDay, e => e.DurationBand),
            by_interaction = Tally(byDay, e => e.Interaction),
            by_lifecycle = Tally(byDay, e => e.Lifecycle),
            by_sector = Tally(byDay, e => e.Sector),
        };
    }
}
