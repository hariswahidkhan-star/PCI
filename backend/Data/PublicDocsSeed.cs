using PCI.Backend.Core;
using B = PCI.Backend.Core.SimplePdf.Block;

namespace PCI.Backend.Data;

/// <summary>
/// Seeds the Public Downloads Centre with a substantive CORE set of governance, candidate, route, consumer,
/// privacy, marketing, IP and compliance documents, generated as accessible selectable-text PDFs. Every
/// document is published as its own version (is_current) but explicitly carries
/// legal_review_status='external_legal_review_pending' and an on-page notice — PCI must have qualified,
/// jurisdiction-specific counsel review before production launch. The full 87-document suite extends this
/// register; the remaining documents are tracked as outstanding until authored and reviewed.
///
/// Idempotent: a document is only created if its doc_group is absent, so re-boots never duplicate. It also
/// registers any pre-existing candidate PDFs shipped under wwwroot/downloads/ so real materials appear.
/// </summary>
public static class PublicDocsSeed
{
    const string EffectiveDate = "19 July 2026";
    const string ReviewNote =
        "Legal-review status: EXTERNAL LEGAL REVIEW PENDING. This document is published for transparency in " +
        "PCI's founding phase and has not yet been reviewed by qualified counsel in every jurisdiction in which " +
        "PCI operates. It may be updated. Where a summary differs from a full policy, the full policy prevails.";
    const string Disclaimer =
        "PCI is an independent professional certification organisation. Recognition and acceptance of PCI " +
        "credentials may vary by employer, industry, institution and jurisdiction. PCI does not guarantee " +
        "employment, promotion, salary improvement, immigration benefits, licensing eligibility or acceptance " +
        "by any third party. PCI is not currently accredited by ANAB, IAS or any ISO/IEC 17024 accreditation " +
        "body; its framework is developed with reference to ISO/IEC 17024 personnel-certification principles.";

    public record Doc(string Group, string Title, string Category, string? Route, string LegalStatus, (B, string)[] Body);

    public static void Ensure(Db db, string webRoot)
    {
        foreach (var d in Core())
        {
            try
            {
                if (db.QueryOne("SELECT id FROM public_documents WHERE doc_group=?", d.Group) is not null) continue;
                var blocks = new List<(B, string)>
                {
                    (B.H2, "Status and scope"),
                    (B.Body, ReviewNote),
                    (B.Body, Disclaimer),
                    (B.Space, ""),
                };
                blocks.AddRange(d.Body);
                blocks.Add((B.Space, ""));
                blocks.Add((B.Body, "Contact: Project Controls Institute Global, Inc. — hello@projectcontrolsinstitute.org — projectcontrolsinstitute.org/downloads"));
                var subtitle = $"Version 1.0 · Effective {EffectiveDate} · Project Controls Institute Global, Inc.";
                var pdf = SimplePdf.Build(d.Title, subtitle, blocks,
                    $"PCI · {d.Title} · v1.0 · Effective {EffectiveDate}");
                var obj = Storage.Put(pdf, "application/pdf", "public-docs");
                var filename = d.Group + ".pdf";
                db.Execute(@"INSERT INTO public_documents
                    (doc_group,title,description,category,route_key,language,version,status,legal_review_status,visibility,
                     owner,effective_date,published_at,next_review_date,storage_ref,filename,mime,size_bytes,sha256,is_current,created_at)
                    VALUES(?,?,?,?,?, 'en','1.0','published',?, 'public','PCI Certification Governance',?,datetime('now'),?,?,?,?,?,?,1,datetime('now'))",
                    d.Group, d.Title, Summary(d.Body), d.Category, d.Route, d.LegalStatus,
                    EffectiveDate, "19 July 2027", obj.Reference, filename, obj.Mime, obj.SizeBytes, obj.Sha256);
            }
            catch { /* one bad document must not stop the rest */ }
        }
        RegisterExistingPdfs(db, webRoot);
    }

    // Turn the first body paragraph into a short catalogue description.
    static string Summary((B, string)[] body)
    {
        foreach (var (k, t) in body) if (k == B.Body) return t.Length > 260 ? t[..260].TrimEnd() + "…" : t;
        return "PCI governance document.";
    }

    // Register candidate PDFs that already ship in wwwroot/downloads/ so real materials show in the centre.
    static void RegisterExistingPdfs(Db db, string webRoot)
    {
        (string file, string group, string title, string cat)[] known =
        {
            ("candidate-handbook.pdf", "candidate-handbook-pdf", "General Candidate Handbook (PDF)", "candidate-handbooks"),
            ("examination-blueprint.pdf", "examination-blueprint", "Examination Blueprint", "exams"),
            ("sample-questions.pdf", "sample-questions", "Sample Questions", "exams"),
            ("study-guide-course-outline.pdf", "study-guide-course-outline", "Study Guide & Course Outline", "exams"),
            ("glossary-of-terms.pdf", "glossary-of-terms", "Glossary of Terms", "general"),
            ("master-formula-sheet.pdf", "master-formula-sheet", "Master Formula Sheet", "exams"),
            ("code-of-professional-conduct.pdf", "code-of-professional-conduct", "Code of Professional Conduct", "certification-governance"),
            ("candidate-ai-use-policy.pdf", "candidate-ai-use-policy", "Candidate Use of AI Policy", "certification-governance"),
        };
        foreach (var (file, group, title, cat) in known)
        {
            try
            {
                if (db.QueryOne("SELECT id FROM public_documents WHERE doc_group=?", group) is not null) continue;
                var path = System.IO.Path.Combine(webRoot, "downloads", file);
                if (!System.IO.File.Exists(path)) continue;
                var bytes = System.IO.File.ReadAllBytes(path);
                if (bytes.LongLength > Storage.MaxBytes) continue; // too large for the shared store guard
                var obj = Storage.Put(bytes, "application/pdf", "public-docs");
                db.Execute(@"INSERT INTO public_documents
                    (doc_group,title,description,category,language,version,status,legal_review_status,visibility,
                     effective_date,published_at,storage_ref,filename,mime,size_bytes,sha256,is_current,created_at)
                    VALUES(?,?,?,?, 'en','1.0','published','external_legal_review_pending','public',
                     ?,datetime('now'),?,?,?,?,?,1,datetime('now'))",
                    group, title, "Candidate resource published by PCI.", cat,
                    EffectiveDate, obj.Reference, file, obj.Mime, obj.SizeBytes, obj.Sha256);
            }
            catch { }
        }
    }

    static Doc[] Core() => new[]
    {
        new Doc("certification-scheme-and-governance-framework", "PCI Certification Scheme and Governance Framework",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Purpose"),
            (B.Body,"This framework describes how the Project Controls Institute (PCI) governs its certification scheme for the PCI AI Project Leadership Certification Suite — PCI AI Project Controls Leader (PCI PCL-AI), PCI AI Project Finance Leader (PCI PFL-AI) and PCI AI Project Delivery Leader (PCI PDL-AI). It sets out the roles, decision-making, and safeguards that keep certification decisions competence-based, impartial and consistent."),
            (B.H1,"Scheme ownership and independence"),
            (B.Body,"PCI owns and operates the certification scheme as an independent certifying body. Certification decisions are made solely on the basis of a candidate's assessed competence against published requirements. Training and preparation are kept separate from the certification decision so that no candidate can obtain a credential other than by meeting the standard."),
            (B.H1,"Governance bodies"),
            (B.Bullet,"A Certification Governance function maintains the scheme, eligibility rules, examination blueprint and policies."),
            (B.Bullet,"An Impartiality function oversees conflicts of interest and safeguards against commercial or personal pressure on certification decisions."),
            (B.Bullet,"An Appeals and Complaints function provides candidates with an independent route to challenge decisions."),
            (B.H1,"Competence and assessment"),
            (B.Body,"Each certification is defined by a body of knowledge and an examination blueprint that specify the domains assessed and their weighting. Assessment is designed to be valid, reliable, fair and secure. Governed use of AI within the discipline is assessed as a competency, alongside classical technique."),
            (B.H1,"Maintaining certification"),
            (B.Body,"Certified professionals maintain their credential through recertification and continuing professional development on a defined cycle. PCI may suspend or revoke a credential for serious misconduct, misrepresentation, or where certification was obtained improperly, subject to due process and a right of appeal."),
            (B.H1,"Review"),
            (B.Body,"This framework is reviewed periodically and whenever the scheme materially changes. The current version published on the PCI website prevails."),
        }),

        new Doc("certification-decision-and-impartiality-policy", "Certification Decision and Impartiality Policy",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Principle"),
            (B.Body,"Certification decisions are made only on evidence of competence measured against published criteria, free from commercial, personal or political influence. PCI does not allow the sale of a certification decision, and no payment secures a pass."),
            (B.H1,"Separation of duties"),
            (B.Body,"Those who prepare or train candidates do not make the certification decision for those candidates. Assessment materials and results are handled so that a conflicted individual cannot influence an outcome."),
            (B.H1,"Conflicts of interest"),
            (B.Bullet,"Individuals involved in assessment or decisions must declare actual, potential or perceived conflicts."),
            (B.Bullet,"Where a conflict exists, the individual is removed from the relevant decision."),
            (B.Bullet,"Relationships with training providers, partners and sponsors are managed so they cannot affect a decision."),
            (B.H1,"Threats to impartiality"),
            (B.Body,"PCI identifies threats to impartiality — including financial, familiarity, and marketing-partner relationships — and applies controls to reduce them to an acceptable level. Impartiality is monitored on an ongoing basis."),
            (B.H1,"Appeals"),
            (B.Body,"A candidate who believes a decision was affected by partiality or error may appeal through the Candidate Appeals Procedure. Appeals are handled by people not involved in the original decision."),
        }),

        new Doc("examination-security-and-integrity-policy", "Examination Security and Integrity Policy",
            "exams", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Objective"),
            (B.Body,"To protect the validity of PCI examinations and the equal treatment of candidates by safeguarding examination content, controlling delivery, and detecting and responding to misconduct."),
            (B.H1,"Protection of content"),
            (B.Bullet,"Examination questions and answer keys are confidential and are never published or distributed."),
            (B.Bullet,"Access to live content is restricted to authorised roles on a need-to-know basis."),
            (B.Bullet,"Item banks are versioned and rotated; compromised items are retired."),
            (B.H1,"Delivery controls"),
            (B.Body,"Examinations are delivered under identity verification and, where applicable, proctoring. Candidates must follow the Examination Day Rules. Prohibited aids, impersonation, and unauthorised recording or sharing of content are examination misconduct."),
            (B.H1,"Misconduct and response"),
            (B.Body,"Suspected misconduct is investigated fairly and confidentially. Substantiated misconduct may result in invalidation of a result, a bar from re-sitting for a period, or revocation of a credential, subject to a right of appeal."),
            (B.H1,"Candidate use of AI"),
            (B.Body,"Unless an examination expressly permits it, using AI tools to answer examination questions is prohibited. Where AI use is permitted, it is defined in the examination instructions and assessed as governed use."),
        }),

        new Doc("certificate-issuance-suspension-revocation-policy", "Certificate Issuance, Suspension, Revocation, Replacement and Reinstatement Policy",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Issuance"),
            (B.Body,"A certificate is issued only after a candidate meets all requirements for the relevant certification. Each certificate carries a unique credential identifier that can be verified through PCI's public verification service."),
            (B.H1,"Validity"),
            (B.Body,"Certificates are valid for the period stated on the credential, subject to recertification. A lapsed credential is not current until reinstated."),
            (B.H1,"Suspension and revocation"),
            (B.Bullet,"PCI may suspend a credential pending investigation of alleged misconduct or misrepresentation."),
            (B.Bullet,"PCI may revoke a credential obtained through fraud or misrepresentation, or following serious breach of the Code of Conduct."),
            (B.Bullet,"Suspension and revocation follow due process and carry a right of appeal."),
            (B.H1,"Replacement and reinstatement"),
            (B.Body,"A holder may request a replacement certificate for a genuine loss or a correction. A lapsed or suspended credential may be reinstated on meeting the applicable conditions. Verification records are updated to reflect the current status of every credential."),
            (B.H1,"Designation use"),
            (B.Body,"Use of a PCI designation is conditional on holding a current credential and complying with the Credential and Designation Use Policy."),
        }),

        new Doc("credential-and-designation-use-policy", "Credential and Designation Use Policy",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Who may use a PCI designation"),
            (B.Body,"Only a person who currently holds the relevant PCI credential may use its designation (for example, PCI PCL-AI). The right to use a designation ends if the credential lapses, is suspended or is revoked."),
            (B.H1,"How designations must be used"),
            (B.Bullet,"Use the exact designation as issued; do not modify, abbreviate misleadingly, or combine it to imply a status not held."),
            (B.Bullet,"Do not imply accreditation, government recognition or endorsement that PCI does not hold."),
            (B.Bullet,"Do not use a PCI name, logo, certificate or badge to suggest that PCI endorses a product, employer or third party without written permission."),
            (B.H1,"Honorary recognition"),
            (B.Body,"Honorary Fellow (PCI) is an honorary recognition, not an examined certification. It must always be described as honorary and must never be presented as a passed examination or an assessed competency credential."),
            (B.H1,"Misuse"),
            (B.Body,"Misuse of a PCI credential, designation, certificate or badge — including use by a person who does not hold it — may result in suspension or revocation and, where appropriate, further action."),
        }),

        new Doc("recertification-and-cpd-policy", "Recertification and Continuing Professional Development Policy",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Why recertification exists"),
            (B.Body,"Recertification keeps a credential meaningful over time by confirming that a holder maintains current competence, including in the governed use of AI within the discipline."),
            (B.H1,"Cycle and obligations"),
            (B.Bullet,"Certified professionals maintain their credential on a defined recertification cycle."),
            (B.Bullet,"Holders record continuing professional development (CPD) relevant to the certification's domains."),
            (B.Bullet,"PCI may request evidence of CPD and may audit records."),
            (B.H1,"Lapse and reinstatement"),
            (B.Body,"A credential that is not recertified within its cycle lapses and is shown as not current in verification until reinstated under the applicable conditions."),
        }),

        new Doc("general-candidate-handbook", "General Candidate Handbook",
            "candidate-handbooks", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"About this handbook"),
            (B.Body,"This handbook explains, in one place, how PCI certification works from eligibility to credential across the PCI AI Project Leadership Certification Suite. Where it summarises a policy, the full policy prevails."),
            (B.H1,"Choosing a certification"),
            (B.Body,"PCI offers three certifications: PCI AI Project Controls Leader (PCI PCL-AI), PCI AI Project Finance Leader (PCI PFL-AI) and PCI AI Project Delivery Leader (PCI PDL-AI). Each has its own body of knowledge and examination blueprint. You may pursue more than one over time."),
            (B.H1,"Eligibility and application"),
            (B.Body,"Check the eligibility requirements for your chosen certification and route, then apply. Provide accurate information; misrepresentation may invalidate an application or credential."),
            (B.H1,"Preparing and sitting the examination"),
            (B.Body,"Prepare using the published body of knowledge and sample materials. Follow the Examination Day Rules and the Examination Security and Integrity Policy. Identity verification applies."),
            (B.H1,"Results, review and appeals"),
            (B.Body,"You will receive your result through your account. If you believe an error occurred, you may request a result review and, where applicable, appeal."),
            (B.H1,"After you certify"),
            (B.Body,"Use your designation in line with the Credential and Designation Use Policy, and maintain your credential through recertification and CPD."),
            (B.H1,"Support, complaints and accommodations"),
            (B.Body,"Contact PCI for support. If you need a reasonable accommodation, request it in advance under the Reasonable Accommodation and Accessibility Policy. Complaints are handled under the Candidate Complaints Procedure."),
        }),

        new Doc("candidate-code-of-conduct", "Candidate Code of Conduct",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Expected conduct"),
            (B.Bullet,"Provide truthful, accurate information in all dealings with PCI."),
            (B.Bullet,"Follow examination rules and do not engage in or assist misconduct."),
            (B.Bullet,"Respect the confidentiality of examination content."),
            (B.Bullet,"Use PCI credentials and designations honestly and only when entitled."),
            (B.H1,"Prohibited conduct"),
            (B.Body,"Prohibited conduct includes cheating, impersonation, sharing or soliciting examination content, falsifying documents or experience, and misrepresenting a PCI credential or designation."),
            (B.H1,"Consequences"),
            (B.Body,"Breach may lead to invalidation of a result, a bar from re-sitting, or suspension or revocation of a credential, subject to due process and a right of appeal."),
        }),

        new Doc("reasonable-accommodation-and-accessibility-policy", "Reasonable Accommodation and Accessibility Policy",
            "accessibility", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Commitment"),
            (B.Body,"PCI is committed to fair access to certification for candidates with disabilities or specific needs, consistent with maintaining the validity and security of assessment."),
            (B.H1,"Requesting an accommodation"),
            (B.Bullet,"Request accommodations in advance of your examination so they can be arranged."),
            (B.Bullet,"PCI may ask for appropriate supporting information, handled confidentially."),
            (B.Bullet,"Accommodations are designed to provide fair access without giving an unfair advantage."),
            (B.H1,"Examples"),
            (B.Body,"Depending on need and feasibility, accommodations may include additional time, rest breaks, assistive technology, or alternative arrangements. PCI will discuss options with the candidate."),
            (B.H1,"Confidentiality and appeals"),
            (B.Body,"Information provided for an accommodation is used only for that purpose and retained for a limited period. A candidate may appeal an accommodation decision."),
        }),

        new Doc("candidate-complaints-and-appeals-procedure", "Candidate Complaints and Appeals Procedure",
            "certification-governance", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Complaints"),
            (B.Body,"A complaint is an expression of dissatisfaction about PCI's service or process. Submit a complaint to PCI with relevant details. PCI acknowledges complaints and aims to resolve them fairly and promptly."),
            (B.H1,"Appeals"),
            (B.Body,"An appeal is a request to reconsider a decision — for example, an examination result, an eligibility or misconduct decision, or a certification decision. Submit an appeal within the stated period, setting out the grounds."),
            (B.H1,"How appeals are handled"),
            (B.Bullet,"Appeals are reviewed by people not involved in the original decision."),
            (B.Bullet,"PCI does not penalise a candidate for raising a good-faith complaint or appeal."),
            (B.Bullet,"PCI communicates the outcome and the reasons."),
            (B.H1,"External rights"),
            (B.Body,"Nothing in this procedure limits any rights you may have under applicable law in your jurisdiction."),
        }),

        new Doc("honorary-recognition-policy", "Honorary Recognition Policy",
            "application-routes", "honorary", "external_legal_review_pending", new (B,string)[]{
            (B.H1,"What honorary recognition is"),
            (B.Body,"Honorary Fellow (PCI) is a board-conferred honorary recognition of distinguished contribution to the profession. It is HONORARY RECOGNITION and is clearly separate from PCI's examined certifications."),
            (B.H1,"What it is not"),
            (B.Bullet,"It does not involve an examination and does not certify assessed competence."),
            (B.Bullet,"It does not carry the same assessment status as an examination-certified credential."),
            (B.Bullet,"It is not purchased; the honorary decision is not for sale."),
            (B.H1,"How it must be described"),
            (B.Body,"Every honorary document, page, application, letter, certificate, badge and verification record states 'Honorary Recognition' or 'Honorary Certificate'. An honorary recipient must never be described as having passed an examination or completed the normal competency assessment."),
            (B.H1,"Suitability and identity"),
            (B.Body,"The board may require limited identity verification (one recent passport-style photograph and one valid government-issued ID) and a limited suitability declaration. Disclosed information is reviewed confidentially, fairly and in context, and does not automatically result in rejection."),
            (B.H1,"Review and withdrawal"),
            (B.Body,"Honorary recognition is conferred at the board's discretion and may be withdrawn for cause, with a right to explain and to seek reconsideration."),
        }),

        new Doc("honorary-identity-verification-privacy-notice", "Honorary Identity Verification Privacy Notice",
            "privacy-and-legal", "honorary", "external_legal_review_pending", new (B,string)[]{
            (B.H1,"What we collect and why"),
            (B.Body,"For honorary recognition, PCI may collect one recent photograph, one valid government-issued ID, and a limited suitability declaration, solely to verify identity and consider suitability for honorary recognition."),
            (B.H1,"How we handle it"),
            (B.Bullet,"Access is restricted to the individuals who must review it."),
            (B.Bullet,"Identity documents are retained only for as long as necessary and then securely deleted."),
            (B.Bullet,"Disclosed suitability information is reviewed confidentially and is not published."),
            (B.H1,"The suitability declaration"),
            (B.Body,"PCI uses a limited declaration: to the best of your knowledge, you disclose any criminal conviction or current formal criminal proceeding that you are legally required to disclose and that may be materially relevant to PCI's consideration of your suitability. Disclosure is reviewed confidentially, fairly and in context and will not automatically result in rejection."),
            (B.H1,"Your rights"),
            (B.Body,"You have the right to be informed, to explain any disclosure, to confidential review, and to request access to or deletion of your information, subject to applicable law."),
        }),

        new Doc("fees-and-payment-policy", "Fees and Payment Policy",
            "fees-and-refunds", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Transparent fees"),
            (B.Body,"Before you pay, PCI discloses the certification and route selected, the applicable fees (which may include application, examination, membership, retake and recertification fees), any tax or VAT, any discount applied, and the net amount payable."),
            (B.H1,"Payment"),
            (B.Bullet,"Fees are payable in the currency and by the methods shown at checkout."),
            (B.Bullet,"Where manual payment is offered, payment evidence may be required before access is granted."),
            (B.Bullet,"A discount code applies only within its stated terms and validity."),
            (B.H1,"Consequences and refunds"),
            (B.Body,"Rejection of an application, cancellation and rescheduling are governed by the Cancellation, Rescheduling and Refund Policy. Certificate validity and recertification obligations are set out in the relevant governance policies."),
        }),

        new Doc("cancellation-rescheduling-and-refund-policy", "Cancellation, Rescheduling and Refund Policy",
            "fees-and-refunds", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Scope"),
            (B.Body,"This policy explains when fees may be cancelled, rescheduled or refunded. It operates alongside any mandatory consumer rights you have under applicable law, which it does not remove."),
            (B.H1,"Cancellation and refunds"),
            (B.Bullet,"Refund eligibility depends on the stage reached and the fee type; some fees become non-refundable once a service has been delivered."),
            (B.Bullet,"Where a refund is due, it is made to the original payment method."),
            (B.Bullet,"Request cancellation or a refund through PCI, quoting your reference."),
            (B.H1,"Rescheduling"),
            (B.Body,"Examination scheduling operates within a defined window from payment. Rescheduling within the window may be permitted; a lapsed window may require a new registration."),
            (B.H1,"Chargebacks and disputes"),
            (B.Body,"If you believe you have been charged in error, contact PCI first so it can be resolved. PCI cooperates with legitimate payment-dispute processes."),
        }),

        new Doc("global-privacy-policy", "Global Privacy Policy",
            "privacy-and-legal", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Our approach"),
            (B.Body,"PCI processes personal data lawfully, fairly and transparently, and only as needed to operate certification, membership and related services. This policy is a global notice; it does not by itself guarantee compliance in every jurisdiction, and specific notices apply to specific activities."),
            (B.H1,"What we collect"),
            (B.Bullet,"Identity and contact details, professional and eligibility information, and application content."),
            (B.Bullet,"Examination, result and credential records."),
            (B.Bullet,"Payment-related information processed through payment providers."),
            (B.Bullet,"Where required, identity-verification information and, for honorary recognition, limited suitability information."),
            (B.H1,"How we use and share it"),
            (B.Body,"We use personal data to deliver and verify certification, communicate with you, meet legal obligations and improve our services. We share data with service providers (processors) under contract, and only as necessary. We do not sell personal data."),
            (B.H1,"International transfers"),
            (B.Body,"Because candidates are international, personal data may be processed in more than one country under appropriate safeguards. Candidate jurisdictions include, among others, Saudi Arabia, the United States and Pakistan; PCI is undertaking jurisdiction-specific review before production launch."),
            (B.H1,"Retention, security and your rights"),
            (B.Body,"We keep personal data only as long as necessary and then securely delete it. We apply access controls and safeguards. Subject to applicable law, you may request access, correction, deletion or restriction, and may object to certain processing."),
            (B.H1,"Minors"),
            (B.Body,"PCI's services are intended for professionals. Where a minor applicant is permitted, additional protections and consents apply under the Children and Minor Applicants Policy."),
        }),

        new Doc("data-retention-and-secure-deletion-policy", "Data Retention and Secure Deletion Policy",
            "privacy-and-legal", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Principle"),
            (B.Body,"PCI keeps personal data only for as long as necessary for the purpose for which it was collected or as required by law, and then securely deletes or anonymises it."),
            (B.H1,"Retention"),
            (B.Bullet,"Credential and verification records are retained to support the integrity of the public register."),
            (B.Bullet,"Identity-verification documents are retained only for a limited period after their purpose is met."),
            (B.Bullet,"Application, examination and support records are retained for defined periods appropriate to their purpose."),
            (B.H1,"Secure deletion"),
            (B.Body,"When a retention period ends, data is securely deleted or irreversibly anonymised. Backups are managed so that deleted data is not indefinitely retained."),
        }),

        new Doc("advertising-and-marketing-claims-policy", "Advertising and Marketing Claims Policy",
            "marketing-partners", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Truthful claims only"),
            (B.Body,"PCI and its partners make only claims that are accurate and supportable. PCI does not represent its credentials as accredited, ISO-certified, government-approved, universally recognised, or as guaranteeing employment, promotion, salary, immigration or licensing outcomes, unless reliable written evidence supports the precise wording."),
            (B.H1,"Prohibited claims"),
            (B.Bullet,"'Internationally recognised', 'globally accredited', 'government approved', 'ISO-accredited' — unless evidenced."),
            (B.Bullet,"'Guaranteed employment/promotion/salary', 'guaranteed recognition'."),
            (B.Bullet,"'Equivalent to' or 'better than' another body's credential, or 'accepted by all employers'."),
            (B.H1,"Comparisons"),
            (B.Body,"Comparisons with other bodies are informational and must not claim equivalence, superiority, partnership or endorsement without evidence. PCI states its accreditation status plainly."),
            (B.H1,"Partners"),
            (B.Body,"Marketing partners must accept binding terms prohibiting unsupported claims and misuse of PCI names, logos, certificates and badges. Breach may end the partner relationship."),
        }),

        new Doc("intellectual-property-and-ai-use-policy", "Intellectual Property and AI Use Policy",
            "privacy-and-legal", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Ownership"),
            (B.Body,"PCI content — including bodies of knowledge, examination items, handbooks, policies and templates — is PCI's intellectual property or is used under licence. Examination questions and answer keys are confidential and are never published."),
            (B.H1,"Third-party content"),
            (B.Bullet,"PCI does not publish copied or closely reproduced material from other bodies, publishers, standards or universities without permission."),
            (B.Bullet,"Paid standards and copyrighted third-party books are not distributed in the public download centre."),
            (B.Bullet,"Third-party marks referenced for comparison remain the property of their owners."),
            (B.H1,"Governed use of AI"),
            (B.Body,"Where PCI uses AI to help produce content, the output is subject to human review before publication. Automated processing that affects a person is subject to human oversight. Candidates may use AI only where an assessment expressly permits it."),
        }),

        new Doc("sanctions-and-restricted-party-compliance-policy", "Sanctions and Restricted-Party Compliance Policy",
            "privacy-and-legal", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Commitment"),
            (B.Body,"PCI seeks to comply with applicable sanctions and export-control laws. PCI does not knowingly provide services to individuals or entities where doing so would breach applicable sanctions or restricted-party requirements."),
            (B.H1,"Controls"),
            (B.Bullet,"PCI may screen applicants and payments against applicable restricted-party requirements."),
            (B.Bullet,"PCI may decline or withdraw service where compliance requires it."),
            (B.Bullet,"PCI keeps this policy under review as its operations and jurisdictions develop."),
            (B.H1,"Anti-bribery and anti-fraud"),
            (B.Body,"PCI prohibits bribery, corruption and fraud in connection with its certification and payments, and provides a route to report concerns confidentially."),
        }),

        new Doc("accessibility-statement", "Accessibility Statement",
            "accessibility", null, "external_legal_review_pending", new (B,string)[]{
            (B.H1,"Our aim"),
            (B.Body,"PCI aims to make its website, documents and services usable by as many people as possible, including people who use assistive technology."),
            (B.H1,"What we do"),
            (B.Bullet,"We publish documents as selectable text with headings, page numbers and version and effective dates."),
            (B.Bullet,"We aim for sufficient colour contrast and logical reading order."),
            (B.Bullet,"We provide reasonable accommodations for examinations on request."),
            (B.H1,"Feedback"),
            (B.Body,"If you encounter an accessibility barrier, contact PCI so we can help and improve. We treat accessibility as an ongoing commitment, not a one-time exercise."),
        }),
    };
}
