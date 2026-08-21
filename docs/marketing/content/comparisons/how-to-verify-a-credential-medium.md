---
platform:      Medium
type:          guide
title:         How to verify a credential before you trust the claim
meta:          How to verify a credential: the ID, the issuer's own register, the state it returns, all 64 characters of the hash, and the scope question underneath.
primary_kw:    how to verify a credential *
secondary_kw:  credential verification, register states, certificate of attendance, honorary recognition
pillar:        Certification and careers
credential:    suite
target_domain: credentialfinder.org
canonical:     canonical -> /how-to-verify-a-credential (credentialfinder.org original)
compares:      [cost and scheduling credentials, accountancy and finance qualifications, audit credentials, project management certifications]
schema:        Article + FAQPage
word_count:    1,520
hashtags:      #ProjectControls #ProjectManagement #PMO #ProjectFinance
ab_id:         n/a — comparison plan batch 7
---

# How to verify a credential before you trust the claim

How to verify a credential, in order: take the ID and the exact designation off the certificate, reach the issuing body's own register yourself rather than through a link you were sent, and read the state it returns. Where the issuer publishes a hash of the file, recompute it and compare all 64 characters. The document proves nothing on its own.

credentialfinder.org is operated by Project Controls Institute Global, which awards the PCL-AI, PFL-AI and PML-AI credentials described here. That is worth knowing before you read a page about checking credentials, because a body telling you to verify independently and owning the site you are reading is a fact you are entitled to weigh.

## What verification actually means

A certificate is a rendering. The credential is a record held by the body that issued it, and verification is the act of asking that body's own system about that record.

Everything that follows works on any credential in any field, issued by anyone. None of it depends on who the issuer is.

## The check, in five steps

**1. Take three things off the document.** The credential ID, the exact designation word for word, and the issue and expiry dates if they are printed. Designations from a single body can differ by one word and represent different examinations, so copy rather than paraphrase.

**2. Reach the register yourself.** Not a link the holder emailed you. Type the issuing body's own address, or scan the code printed on the document and read the domain it resolves to before you trust the page. If the address is not on the issuer's own domain, that is the finding.

**3. Read the state, not a yes or no.** Active, expired, suspended and revoked mean four different things to a hiring decision, and a system that answers only "found" is telling you less than it appears to.

**4. Do the tamper check if one is offered.** Recompute the published hash on the file you hold, and compare the whole string.

**5. Read the designation against the job.** A real record for the wrong scope is still the wrong credential.

| Check | The question it answers | What a weak answer looks like |
|---|---|---|
| Credential ID in the issuer's register | Does this record exist? | A confirmation page on a domain that is not the issuer's |
| The state returned | Is it current, lapsed, withheld or withdrawn? | A single "verified" with no state |
| Expiry computed at query time | Is it current *today*? | A stored status that never lapses |
| Published file hash | Is this the file that was issued? | No hash, or a hash truncated to a few characters |
| Designation and scope | Was this person examined on what I need? | A designation that appears nowhere in the published syllabus |

## The arithmetic of a tamper check

Where an issuer publishes the SHA-256 hash of the file it issued, the check is a comparison of two 64-character strings.

`shasum -a 256 certificate.pdf` on macOS or Linux, or `Get-FileHash -Algorithm SHA256 .\certificate.pdf` in PowerShell, gives you the hash of the file in front of you.

People compare the first eight characters and stop. Eight hex characters is 32 bits, which is 16⁸ = **4,294,967,296** possibilities. A forger who wants a specific eight-character prefix has to search a space of about four billion variants of a document, which is a laptop-scale problem, and a birthday-style collision on the same prefix needs only about 65,536 attempts. The full string is 2²⁵⁶ and nobody is grinding through that.

Comparing all 64 characters instead of eight costs five seconds and removes the entire attack.

## Expiry is computed, not stored

A record whose stored status still reads active, carrying an expiry of 30 June, must come back **expired** on 1 July.

If a register hands you an "active" for a credential that lapsed a year ago, it is reporting a stored field rather than answering the question you asked. Test it on a record you know has lapsed before you rely on it for a hire.

## What a verified record still does not tell you

It proves the record is real. It does not tell you the credential covers the work.

That is a scope question, and scope is where the categories genuinely differ.

| Category | What it sets out to examine |
|---|---|
| Cost and scheduling credentials | Estimating, cost control, planning, schedule analysis and the economics of the asset life cycle |
| The established project controls certifications | Measurement, progress, forecasting and the control of delivery |
| Accountancy and finance qualifications | Recognition, measurement, disclosure, financial analysis and control |
| Audit credentials | Governance, risk, control and the assurance process |
| Project management certifications | Managing projects across people, process and the business environment |
| Product certifications for scheduling software | Operating a specific tool rather than the discipline behind it |
| Chartered routes in surveying and engineering | Assessed professional competence and chartered standing |

Each of these examines its own subject thoroughly and is built to. A scope boundary is a design decision, not a shortcoming. The mistake is not the credential; it is reading a verification result as an answer to a question the credential never set out to answer.

## Two documents people confuse

A **certificate of attendance** records that somebody was present for something. It is a legitimate document and it is not a credential.

**Honorary or member recognition** is recognition, not an examination. PCI holds honorary recognition in a separate register with its own number space and never reports it as a passed examination, and records created on test accounts are not findable at all, so a demonstration can never mint something verifiable. Ask any body how it draws that line; the answer is informative whichever way it goes.

## What this body does not claim

PCI is a new, independent certifying body. It is not accredited by ANAB, UKAS, IAS or any other ISO/IEC 17024 accreditation body, and does not claim to be. The scheme is built with reference to ISO/IEC 17024 principles. Read the published Body of Knowledge before you decide anything.

PCI publishes no pass rates, no salary outcomes and no holder counts. The [full Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge) can be read before anyone pays anything, which is the check that matters most and the one fewest bodies make easy.

## How to decide what to do with the answer

If the register returns active and the designation matches the work, you are done.

If your procurement rule or your regulator requires an accredited credential, that question outranks this one. Settle it first, because a clean register entry for an unaccredited credential still will not satisfy the rule, and contract stage is an expensive place to find out. If the answer is that you need an accredited route today, take the accredited route — that is the honest advice and it points away from us.

If the register returns suspended, treat it as an open question rather than a decline, and ask the issuer what the state means in their scheme. Bodies use it differently.

## Frequently asked questions

**Is a QR code on the certificate enough?**
It is a convenience, not a proof. The code encodes an address, and an address on a document can be printed by whoever printed the document. Scan it, then read the domain it resolves to. If it is the issuing body's own domain you have saved yourself typing an ID; if it is not, you have learned something more useful.

**What if the body has no online register?**
Some issuers confirm only by replying to an email. That is slower and it is not worthless. The test is whether a third party can confirm the claim without the holder sitting in the middle of it, and an email from the issuer's own domain meets that test even if a register would be quicker.

**Does an expired credential mean somebody lied?**
No. It means the record has passed its expiry date, which is common, unremarkable and often a matter of timing rather than lapsed competence. Ask when they intend to recertify. What matters is that the register told you the truth rather than reporting a stale field as current.

**Can I verify a credential without the holder's ID number?**
Sometimes. Some registers search by name, which is convenient and produces ambiguity where names repeat. An ID-based lookup is the more reliable check, and asking a candidate for the ID is a normal request that a genuine holder answers in seconds.

**Does verification tell me anything about quality?**
Only that the record exists and is current. Quality is a syllabus question, answered by reading what the body publishes about what it examines and how it decides. Verification and evaluation are two separate jobs, and doing the first well is no substitute for doing the second at all.

---

*First published on credentialfinder.org; the canonical for this article points there, and Medium links are nofollow, so this republish exists for readers rather than for link equity. The hash arithmetic is general cryptography, not a claim about any scheme. PCI publishes certification requirements; nothing here is legal, tax or accounting advice. No other certifying body, awarding organisation or credential is named here, by name or by description.*

*Internal links: one in the body, to [the full Body of Knowledge](https://projectcontrolsinstitute.org/body-of-knowledge), placed on the sentence saying the syllabus can be read before payment, because that is the one claim in the piece a reader should be able to test immediately. The register link was not added as well: this is an off-estate republish and two links to one domain in one article is the pattern to avoid. On the credentialfinder.org original, the same-domain internal links are [what accreditation means](/what-accreditation-means) anchored "what accreditation does and does not tell you", [how to tell a credential from a certificate of attendance](/credential-or-certificate-of-attendance) anchored "the difference between the two documents", and [how to judge a certifying body that is new](/judging-a-certifying-body-that-is-new) anchored "judging a body without a track record". None of them may link to another awarding body's website.*
