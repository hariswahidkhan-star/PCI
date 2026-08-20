---
platform:      LinkedIn post
type:          linkedin-post
title:         How to verify a PCI AI credential in under a minute
meta:          A certificate PDF is a picture of a claim. The register returns one of four states, plus a SHA-256 hash you can recompute on the file in front of you.
primary_kw:    verify a PCI credential
secondary_kw:  credential verification, PCL-AI, PFL-AI, PML-AI
pillar:        Certification and careers
credential:    suite
target_domain: credentialfinder.org
canonical:     original
schema:        Article
word_count:    317
hashtags:      #ProjectControls #ProjectManagement #PMO
ab_id:         AB-00114
---

# How to verify a PCI AI credential in under a minute

**Post body (1,870 characters):**

A certificate PDF proves nothing on its own. It is a picture of a claim, and pictures are easy to edit. Checking the real record takes about forty seconds.

To verify a PCI credential, take the ID off the certificate and put it into the public register. What comes back is a state, not a yes or no, and the four states are the whole point.

Active means current at the moment you asked. Expired is computed against the expiry date when you ask, so a record whose stored status still reads active but whose expiry has passed comes back expired anyway.

Suspended is its own answer and never a quiet pass. Revoked means it was withdrawn, and the record stays in the register saying so.

Do not type a URL somebody emailed you. The certificate carries a QR code encoding the verification address for that credential ID. Scan it off the document you were given.

Then the tamper check, which is the part most people skip. The register returns the SHA-256 hash of the exact PDF that was issued. Recompute it on the file in your hand and compare all 64 characters.

shasum -a 256 certificate.pdf on macOS or Linux
Get-FileHash -Algorithm SHA256 .\certificate.pdf in PowerShell

Change one pixel and the hash changes completely. Match it and you hold the issued document.

Two things worth knowing. Honorary recognition sits in a separate register with its own number space and is never reported as a passed examination. Records created on test accounts are not findable at all, so a demonstration run can never mint something verifiable.

Read the designation carefully too. PCI AI Project Controls Leader (PCL-AI) covers 13 domains and 61 knowledge areas. PCI AI Project Finance Leader (PFL-AI) covers 16 domains and 61 knowledge areas. PCI Project Management Leader – AI (PML-AI) covers 16 domains and 63 knowledge areas. Three different examinations, three different claims.

#ProjectControls #ProjectManagement #PMO

**First comment:** The public register, and what each verification state means for a hiring decision: https://credentialfinder.org/verify

---

*PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [verify a PCI credential](https://credentialfinder.org/verify) with the anchor "check a credential record independently", and [PCL-AI certification](https://projectcontrolsinstitute.org/pcl-ai-certification) with the anchor "what the PCL-AI credential covers".*
