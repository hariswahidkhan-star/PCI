---
platform:      LinkedIn carousel
type:          carousel
title:         How to verify a credential: the register, not the PDF
meta:          Thirteen slides on checking a credential properly: the ID, the issuing body's own register, the four states it should return and the file hash.
primary_kw:    verify a credential *
secondary_kw:  credential verification, register states, certificate of attendance, honorary recognition
pillar:        Certification and careers
credential:    suite
target_domain: credentialfinder.org
canonical:     derived from /how-to-verify-a-credential (credentialfinder.org original) — built for the carousel, not a copy
compares:      [cost and scheduling credentials, accountancy and finance qualifications, audit credentials, project management certifications]
schema:        Article
word_count:    980
hashtags:      #ProjectControls #ProjectManagement #PMO
ab_id:         n/a — comparison plan batch 7
---

# How to verify a credential: the register, not the PDF

*LinkedIn document post — 13 slides, 1080 × 1350. No link in the body; the link goes in the first comment.*

**Post caption (the first two lines carry the post):**

A certificate PDF proves nothing on its own. It is a picture of a claim, and pictures are easy to edit.
Thirteen slides on checking the record behind it, in about a minute, whoever issued it.

Disclosure before you read: credentialfinder.org is operated by Project Controls Institute Global, which awards the PCL-AI, PFL-AI and PML-AI credentials. PCI is new and not accredited. Read the syllabus and judge it.

---

**Slide 1 — A certificate is a picture of a claim**

The document is a rendering. The credential is a record held by the body that issued it. Verification means asking that body's own system about that record, on a route you found yourself.

Everything below works on any credential, in any field, issued by anyone.

**Slide 2 — Three things to take off the document**

The credential ID. The exact designation, word for word. The issue date and expiry date if one is printed.

The designation matters more than people expect. Two credentials from one body can differ by a single word and be different examinations.

**Slide 3 — The tamper check, and its arithmetic**

Where the issuer publishes a cryptographic hash of the file it issued, recompute it on the file in your hand and compare **all 64 characters**.

`shasum -a 256 certificate.pdf` on macOS or Linux
`Get-FileHash -Algorithm SHA256 .\certificate.pdf` in PowerShell

Comparing only the first eight characters is 32 bits of check: 16⁸ = **4,294,967,296** variants, a number a laptop can grind through. The full string is 2²⁵⁶.

Change one pixel and the hash changes completely. Match all 64 and you are holding the file that was issued.

**Slide 4 — Reach the register yourself**

Do not use a verification link somebody sent you. Type the issuing body's own address, or scan the code printed on the document and read the domain it resolves to before you trust the page.

If the address is not on the issuing body's own domain, that is the finding. Stop there.

**Slide 5 — A register should return a state, not a yes**

Four states carry four different meanings for a hiring decision:

| State | What it means |
|---|---|
| Active | Current at the moment you asked |
| Expired | Past its expiry date, computed when you ask |
| Suspended | Withheld while something is unresolved — never a quiet pass |
| Revoked | Withdrawn, with the record kept saying so |

A system that answers only "found" or "not found" is telling you less than it looks.

**Slide 6 — Expiry is computed, not stored**

A record whose stored status still reads active, with an expiry of 30 June, must come back **expired** on 1 July.

If a register can hand you an "active" for a credential that lapsed a year ago, it is reporting a field rather than answering your question.

**Slide 7 — What a register cannot tell you**

It proves the record is real. It does not tell you the credential fits the role you are hiring for.

That is a scope question, and scope is what the categories differ on.

**Slide 8 — Scope, at category level**

| Category | What it sets out to examine |
|---|---|
| Cost and scheduling credentials | Estimating, cost control, planning, schedule analysis |
| Accountancy and finance qualifications | Recognition, measurement, disclosure, financial control |
| Audit credentials | Governance, risk, control and the assurance process |
| Project management certifications | Managing projects across people, process and the business environment |

Each examines its own subject thoroughly and is built to. Read the register answer next to the job description, not instead of it.

**Slide 9 — A credential is not a certificate of attendance**

A certificate of attendance records that somebody was present. A credential records that somebody was examined against a published standard and met a decision rule.

Both are legitimate documents. They answer different questions, and the wording on the page is where you tell them apart.

**Slide 10 — Honorary recognition is not an examination**

PCI holds honorary recognition in a separate register with its own number space, and never reports it as a passed examination. Records created on test accounts are not findable at all, so a demonstration run cannot mint something verifiable.

Ask any body how it separates the two. The answer is informative either way.

**Slide 11 — When there is no register at all**

Some bodies confirm a credential only by replying to an email. That is a slower check, not a worthless one, but it puts a person between you and the record.

What matters is whether a third party can confirm the claim without the holder in the loop.

**Slide 12 — When verification is not your first question**

If your procurement rule or your regulator requires an accredited credential, settle that first. A clean register entry for an unaccredited credential still will not satisfy the rule, and contract stage is an expensive place to discover it.

PCI is a new, independent certifying body. It is not accredited by ANAB, UKAS, IAS or any other ISO/IEC 17024 accreditation body, and does not claim to be. The scheme is built with reference to ISO/IEC 17024 principles.

**Slide 13 — The one-minute checklist**

1. ID and exact designation off the document.
2. The issuing body's own register, reached by you.
3. The state returned, read as a state.
4. All 64 characters of the hash, where one is published.
5. The designation against the job, which is a scope question.

---

#ProjectControls #ProjectManagement #PMO

**First comment:** The public register and what each verification state means for a hiring decision: https://projectcontrolsinstitute.org/verify

---

*The hash arithmetic is general cryptography, not a claim about any scheme. PCI publishes certification requirements; nothing here is legal, tax or accounting advice. No other certifying body, awarding organisation or credential is named on any slide, by name or by description.*

*Internal links: one, in the first comment — [the public register](https://projectcontrolsinstitute.org/verify) with the anchor "what each verification state means for a hiring decision", because slides 5 and 6 describe a check and that page is where it is made. A second link to the same domain is not placed: the carousel raises no question a syllabus page answers, and two links to one domain in one post is the pattern the link architecture exists to prevent. When the credentialfinder.org original publishes at /how-to-verify-a-credential, it takes the first comment with the anchor "the full walkthrough, step by step", and the register link moves to a follow-up comment. No link to any other awarding body's register, ever — a link is a naming and it hands over the referral as well.*
