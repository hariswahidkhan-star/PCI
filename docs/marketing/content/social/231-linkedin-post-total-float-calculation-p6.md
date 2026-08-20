---
platform:      LinkedIn post
type:          linkedin-post
title:         How total float is actually calculated in Primavera P6
meta:          Total float is a property of a path, not an activity. The back-pass arithmetic, and the three P6 settings that change the number before you report it.
primary_kw:    total float calculation P6
secondary_kw:  total float formula, must finish by, longest path, negative float
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    334
hashtags:      #ProjectControls #Primavera #Scheduling #PMO
ab_id:         AB-00185
---

# How total float is actually calculated in Primavera P6

**Post body (1,810 characters):**

Total float is not a property of an activity. It is the gap between two chains of work, and P6 will hand you a number for it that changes with settings most people have never opened.

The total float calculation itself is not the hard part. Total float = late start − early start = late finish − early finish. The forward pass gives the early dates. The backward pass gives the late dates, and the backward pass has to start from somewhere. That starting point is the whole argument.

Three activities, finish-to-start, no lag. A is 15 days, B is 20, C is 10.

Forward pass: A runs 0–15, B runs 15–35, C runs 35–45.

Back-pass from day 45, the network's own finish: total float is 0 on all three. The chain is critical and the job needs 45 days.

Now put a Must Finish By date of day 40 on the project. The back-pass starts from 40 instead: C is late 30–40, B is late 10–30, A must have started on day −5. Total float = 40 − 45 = −5 days on every activity.

No logic changed. No duration changed. Nobody worked slower. The float moved five days because you asked P6 a different question.

Three settings then decide the number on your report.

Compute Total Float as Start Float, Finish Float, or the Smallest of the two. They agree only where an activity's early and late dates sit on one uninterrupted calendar. Across a calendar boundary they diverge, and Smallest is the conservative read.

Calculate float based on the finish date of each project, or of all open projects. Open a second project and float in the first can change while you watch.

Critical defined by a float threshold, or by longest path. A hard constraint in the middle of the network makes those two lists disagree, and only longest path answers "what moves the end date".

Say which one you used before you say how much float there is.

#ProjectControls #Primavera #Scheduling #PMO

**First comment:** The full back-pass, free float versus interfering float, and how float erosion is tracked month to month: https://projectcontrolsinstitute.org/total-float

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [total float](https://projectcontrolsinstitute.org/total-float) with the anchor "total float definition and worked network", and [critical path method](https://projectcontrolsinstitute.org/critical-path-method) with the anchor "how the forward and backward pass produce the critical path".*
