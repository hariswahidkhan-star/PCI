namespace PCI.Backend.Data;

/// <summary>
/// Seeds the Communications Centre with default (editable) email sender profiles and the platform-event
/// trigger catalogue. Idempotent — rows are only inserted when absent, so an operator's later edits are
/// never clobbered. `backend_wired` marks which trigger codes a real backend event already emits; the rest
/// are catalogued (so an admin can pre-configure them) but flagged in the UI as "development required".
/// No secrets are seeded here — provider credentials live only in Render environment variables.
/// </summary>
public static class CommsSeed
{
    public static void Ensure(Db db)
    {
        // (key, name, from_email, category)
        var senders = new (string key, string name, string from, string cat)[]
        {
            ("general", "General information", "hello@projectcontrolsinstitute.org", "operational"),
            ("students", "Student services", "students@projectcontrolsinstitute.org", "operational"),
            ("certifications", "Certification services", "certifications@projectcontrolsinstitute.org", "operational"),
            ("membership", "Membership", "membership@projectcontrolsinstitute.org", "operational"),
            ("exams", "Examination services", "exams@projectcontrolsinstitute.org", "operational"),
            ("finance", "Finance", "finance@projectcontrolsinstitute.org", "transactional"),
            ("support", "Customer support", "support@projectcontrolsinstitute.org", "support"),
            ("privacy", "Privacy", "privacy@projectcontrolsinstitute.org", "legal"),
            ("marketing", "Marketing", "news@projectcontrolsinstitute.org", "marketing"),
            ("institutions", "Institution support", "institutions@projectcontrolsinstitute.org", "operational"),
            ("partners", "Marketing-partner support", "partners@projectcontrolsinstitute.org", "operational"),
            ("no-reply", "No-reply", "no-reply@projectcontrolsinstitute.org", "transactional"),
        };
        foreach (var (key, name, from, cat) in senders)
            db.Execute(@"INSERT INTO comm_sender_profiles(`key`,name,display_name,from_email,reply_to,category,is_default,active,approval_status)
                SELECT ?,?,?,?,?,?,?,1,'approved' WHERE NOT EXISTS(SELECT 1 FROM comm_sender_profiles WHERE `key`=?)",
                key, name, "Project Controls Institute", from, from, cat, key == "no-reply" ? 1 : 0, key);

        // Trigger catalogue: (code, name, group, backend_wired). wired=1 for events an existing backend
        // path already fires today; the rest are catalogued for configuration with a "needs wiring" flag.
        var t = new List<(string code, string name, string grp, int wired)>();
        void G(string grp, params (string code, string name, int wired)[] items)
        { foreach (var it in items) t.Add((it.code, it.name, grp, it.wired)); }

        G("Account & security",
            ("account.welcome", "Welcome", 1), ("account.email_verification", "Email verification", 0),
            ("account.password_reset", "Password reset", 1), ("account.password_changed", "Password changed", 0),
            ("account.totp_enabled", "TOTP enabled", 0), ("account.totp_disabled", "TOTP disabled", 1),
            ("account.locked", "Account locked", 0), ("account.suspicious_login", "Suspicious login", 0),
            ("account.restored", "Account restored", 0));
        G("Applications",
            ("application.started", "Application started", 0), ("application.incomplete_reminder", "Application incomplete reminder", 0),
            ("application.submitted", "Application submitted", 1), ("application.received", "Application received", 1),
            ("application.missing_documents", "Missing documents", 0), ("application.info_required", "Additional information required", 0),
            ("application.approved", "Application approved", 1), ("application.rejected", "Application rejected", 1),
            ("application.reopened", "Application reopened", 0), ("application.deadline_extended", "Application deadline extended", 0));
        G("Honorary route",
            ("honorary.nomination_received", "Nomination received", 1), ("honorary.shortlisted", "Candidate shortlisted", 1),
            ("honorary.idv_requested", "Identity verification requested", 1), ("honorary.info_requested", "Additional information requested", 0),
            ("honorary.approved", "Honorary approval", 1), ("honorary.rejected", "Honorary rejection", 1),
            ("honorary.certificate_issued", "Honorary certificate issued", 1));
        G("Membership",
            ("membership.payment_required", "Membership payment required", 0), ("membership.payment_received", "Membership payment received", 1),
            ("membership.activated", "Membership activated", 1), ("membership.expiry_reminder", "Membership-expiry reminder", 0),
            ("membership.expired", "Membership expired", 0), ("membership.renewed", "Membership renewed", 0));
        G("Payments",
            ("payment.invoice_issued", "Invoice issued", 0), ("payment.pending", "Payment pending", 0),
            ("payment.successful", "Payment successful", 1), ("payment.failed", "Payment failed", 0),
            ("payment.manual_received", "Manual payment received", 1), ("payment.manual_approved", "Manual payment approved", 1),
            ("payment.refund_issued", "Refund issued", 1), ("payment.receipt_available", "Receipt PDF available", 1),
            ("payment.fee_waiver_approved", "Fee waiver approved", 1));
        G("Exam eligibility & scheduling",
            ("exam.eligibility_granted", "Exam eligibility granted", 1), ("exam.scheduling_opened", "Exam scheduling opened", 1),
            ("exam.schedule_button_activated", "Schedule Exam button activated", 0), ("exam.scheduling_reminder", "Scheduling reminder", 0),
            ("exam.scheduling_deadline_approaching", "Scheduling deadline approaching", 0), ("exam.scheduling_deadline_expired", "Scheduling deadline expired", 0),
            ("exam.deadline_extended", "Admin extended scheduling deadline", 0), ("exam.scheduling_reopened", "Admin reopened scheduling", 0),
            ("exam.free_authorization", "Free exam authorization granted", 1), ("exam.appointment_reserved", "Exam appointment reserved", 1),
            ("exam.appointment_confirmed", "Exam appointment confirmed", 1), ("exam.appointment_changed", "Exam appointment changed", 1),
            ("exam.appointment_cancelled", "Exam appointment cancelled", 1), ("exam.appointment_reminder", "Exam appointment reminder", 0),
            ("exam.access_instructions", "Exam access instructions", 0));
        G("Exam rescheduling",
            ("resched.requested", "Rescheduling requested", 0), ("resched.approved", "Rescheduling approved", 1),
            ("resched.rejected", "Rescheduling rejected", 0), ("resched.fee_required", "Rescheduling fee required", 0),
            ("resched.fee_waived", "Rescheduling fee waived", 0), ("resched.window_opened", "Rescheduling window opened", 0),
            ("resched.new_appointment_required", "New appointment required", 0), ("resched.new_appointment_confirmed", "New appointment confirmed", 1),
            ("resched.admin", "Admin rescheduled exam", 1), ("resched.provider", "Provider rescheduled exam", 0),
            ("resched.technical_failure", "Technical-failure rescheduling", 0), ("resched.missed", "Missed-exam rescheduling", 0),
            ("resched.exceptional", "Exceptional rescheduling", 0), ("resched.deadline_reminder", "Rescheduling deadline reminder", 0),
            ("resched.deadline_expired", "Rescheduling deadline expired", 0));
        G("Exam incidents & results",
            ("exam.launch_failure", "Exam-launch failure", 0), ("exam.incident_received", "Technical incident received", 0),
            ("exam.incident_review", "Incident under review", 0), ("exam.replacement_attempt", "Replacement attempt granted", 0),
            ("exam.additional_attempt", "Additional attempt granted", 1), ("exam.complimentary_attempt", "Complimentary attempt granted", 1),
            ("exam.completed", "Exam completed", 1), ("exam.result_pending", "Result pending", 1),
            ("exam.result_available", "Result available", 1), ("exam.passed", "Exam passed", 1),
            ("exam.failed", "Exam failed", 1), ("exam.retake_eligibility", "Retake eligibility granted", 0),
            ("exam.appeal_update", "Result appeal update", 0));
        G("Certificates",
            ("cert.generated", "Certificate generated", 1), ("cert.issued", "Certificate issued", 1),
            ("cert.download_available", "Certificate download available", 1), ("cert.corrected", "Certificate corrected", 0),
            ("cert.replaced", "Certificate replaced", 0), ("cert.expiring", "Certificate expiring", 0),
            ("cert.recert_due", "Recertification due", 0), ("cert.suspended", "Certificate suspended", 1),
            ("cert.revoked", "Certificate revoked", 1), ("cert.reinstated", "Certificate reinstated", 1));
        G("Documents & books",
            ("doc.handbook_available", "Candidate Handbook available", 0), ("doc.bok_available", "Body of Knowledge available", 0),
            ("doc.personalized_book_ready", "Personalized book ready", 0), ("doc.assigned", "Document assigned", 1),
            ("doc.updated_published", "Updated document published", 0), ("doc.ack_required", "Mandatory acknowledgement required", 1));
        G("Certuvo",
            ("certuvo.credentials_created", "Credentials created", 1), ("certuvo.access_available", "Access available", 1),
            ("certuvo.provisioning_failed", "Provisioning failed", 0), ("certuvo.credentials_resent", "Credentials resent", 0),
            ("certuvo.access_expiring", "Access expiring", 0));
        G("Support",
            ("support.ticket_created", "Ticket created", 1), ("support.agent_replied", "Agent replied", 1),
            ("support.info_requested", "Additional information requested", 0), ("support.escalated", "Ticket escalated", 0),
            ("support.resolved", "Ticket resolved", 1), ("support.reopened", "Ticket reopened", 0),
            ("support.satisfaction_survey", "Satisfaction survey", 0));
        G("Privacy",
            ("privacy.deletion_received", "Deletion request received", 1), ("privacy.under_review", "Request under review", 0),
            ("privacy.info_required", "Additional information required", 0), ("privacy.approved", "Request approved", 0),
            ("privacy.partially_approved", "Request partially approved", 0), ("privacy.completed", "Request completed", 0));

        foreach (var (code, name, grp, wired) in t)
            db.Execute(@"INSERT INTO comm_triggers(code,name,event_group,backend_wired,email_enabled,whatsapp_enabled,inapp_enabled,sender_profile_key,consent_category,active)
                SELECT ?,?,?,?,1,0,1,?, 'transactional',1 WHERE NOT EXISTS(SELECT 1 FROM comm_triggers WHERE code=?)",
                code, name, grp, wired, DefaultSender(grp), code);
    }

    // Map an event group to a sensible default sender profile.
    static string DefaultSender(string grp) => grp switch
    {
        "Payments" => "finance",
        "Exam eligibility & scheduling" or "Exam rescheduling" or "Exam incidents & results" => "exams",
        "Membership" => "membership",
        "Certificates" => "certifications",
        "Honorary route" => "certifications",
        "Support" => "support",
        "Privacy" => "privacy",
        "Applications" => "students",
        _ => "general",
    };
}
