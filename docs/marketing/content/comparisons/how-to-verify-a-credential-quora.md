---
platform:      Quora
type:          qa-list
title:         How can I check whether someone's certification is real?
meta:          To check whether someone's certification is real, query the issuer's own register with the ID, read the state it returns, and compare all 64 hash characters.
primary_kw:    check whether someone's certification is real *
secondary_kw:  verify a credential, register states, certificate of attendance, credential ID
pillar:        Certification and careers
credential:    suite
target_domain: credentialfinder.org
canonical:     original — written for Quora, not a copy of /how-to-verify-a-credential
compares:      [cost and scheduling credentials, accountancy and finance qualifications, audit credentials, project management certifications]
schema:        FAQPage
word_count:    540
hashtags:      n/a (Quora)
ab_id:         n/a — comparison plan batch 7
---

# How can I check whether someone's certification is real?

To check whether someone's certification is real, check the record rather than the document. A certificate is a rendering of a claim held somewhere else, and the somewhere else is the issuing body's own register. The whole check takes about a minute.

## The check, in five moves

**Take three things off the document.** The credential ID. The exact designation, word for word. The issue and expiry dates if printed. Designations from one body can differ by a single word and be different examinations, so copy rather than paraphrase.

**Reach the register yourself.** Not a verification link the candidate emailed you. Type the issuing body's own address, or scan the code on the document and read the domain it lands on before trusting the page. If it is not the issuer's own domain, you have your answer already.

**Read the state it returns, not a yes or no.** A register worth the name distinguishes four:

| State | What it means for your decision |
|---|---|
| Active | Current at the moment you asked |
| Expired | Past its expiry date, computed when you ask |
| Suspended | Withheld while something is unresolved, never a quiet pass |
| Revoked | Withdrawn, with the record kept saying so |

A record stored as active but carrying an expiry of 30 June must come back expired on 1 July.

**Do the tamper check where one is offered.** Some issuers publish a cryptographic hash of the exact file they issued. Recompute it and compare all 64 characters, not the first few. Eight hex characters is 32 bits: 16⁸ = 4,294,967,296 possibilities, which a laptop grinds through. The full string is 2²⁵⁶.

**Then read the wording.** A certificate of attendance records that somebody was present. A credential records that somebody was examined against a published standard and met a decision rule. Honorary or member designations are recognition, not examinations, and a well-run body keeps them in a separate register and says so.

## What the check cannot tell you

A verified record tells you the credential is real. It does not tell you it fits the job, and that is a scope question.

Cost and scheduling credentials examine estimating, cost control, planning and schedule analysis. Accountancy and finance qualifications examine recognition, measurement and disclosure. Audit credentials examine governance, risk, control and assurance. Project management certifications examine managing projects across people, process and the business environment.

Each examines its own subject thoroughly and is built to, so read the register answer next to the job description.

One case where verification is not your first question at all. If your procurement rule or your regulator requires an accredited credential, settle that before you verify anything, because a clean register entry for an unaccredited credential still will not satisfy the rule.

## Disclosure, because I am one of the bodies in this field

credentialfinder.org is operated by Project Controls Institute Global, which awards the PCL-AI, PFL-AI and PML-AI credentials. PCI is a new, independent certifying body. It is not accredited by ANAB, UKAS, IAS or any other ISO/IEC 17024 accreditation body and does not claim to be. The scheme is built with reference to ISO/IEC 17024 principles. Read the published Body of Knowledge before deciding anything, including about us.

If you want to see what a register that answers with states rather than a yes or no looks like, ours is here: https://projectcontrolsinstitute.org/verify

---

*The hash arithmetic is general cryptography, not a claim about any scheme. PCI publishes certification requirements; nothing here is legal, tax or accounting advice. No other certifying body, awarding organisation or credential is named in this answer, by name or by description.*

*Internal links: one, at the end, to [the public register](https://projectcontrolsinstitute.org/verify) with the anchor "a register that answers with states rather than a yes or no", placed after the answer is complete because a Quora answer that leads with a link is an advertisement. A second link is not added: Quora links are nofollow, this is qualified traffic rather than equity, and two links to one domain in a single answer is the density to avoid. Reciprocal: none — no PCI page should link out to a Quora answer. Never link to another awarding body's register from here; describe the check and let the reader find their own issuer.*
