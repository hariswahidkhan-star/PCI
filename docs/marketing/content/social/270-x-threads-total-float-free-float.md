---
platform:      X / Threads
type:          thread
title:         Total float, free float and the myth of spare days
meta:          Three activities each showing six days of float, and the chain has six days once. Seven posts on how float is calculated, shared and misread.
primary_kw:    total float definition *
secondary_kw:  free float, negative float, critical path method, schedule calendars
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    437
hashtags:      #Scheduling #ProjectControls
ab_id:         AB-00598
---

# Total float, free float and the myth of spare days

*X / Threads thread — 7 posts, each under 280 characters and each able to stand alone. The link sits in the final post. Character counts are for production; X counts any URL as 23 characters, so the live figures run lower.*

**Post 1/7 — the hook** (186 characters)
Three activities in a chain. Each one shows six days of total float.

The chain has six days, once. Whoever reaches them first spends them, and the other two find out at the next update.

**Post 2/7 — the definition** (216 characters)
Total float is the number of days an activity can slip before it delays project completion or breaks an imposed date. It is calculated, not allocated: late start minus early start, from the forward and backward pass.

**Post 3/7 — the arithmetic** (209 characters)
A: days 1–10. B: 11–16. C: 17–24. Contract date day 30.

Backward pass gives LF(C) = 30, so TF(C) = 30 − 24 = 6.
LF(B) = 22, TF(B) = 22 − 16 = 6.
LF(A) = 16, TF(A) = 16 − 10 = 6.
Six days, counted three times.

**Post 4/7 — what happens when someone uses it** (227 characters)
A slips four days. B and C shift with it, so C now finishes on day 28.
Total float on all three = 30 − 28 = 2 days.
B and C consumed nothing and lost four days each. Float belongs to the path, not to the activity displaying it.

**Post 5/7 — free float is the one you own** (229 characters)
Free float is the days an activity can slip without moving the early start of its successor. A and B have none here, because their successors start the next day. C has six. That is the only float a single activity genuinely owns.

**Post 6/7 — two traps** (245 characters)
Negative float is a message about a constraint, not about productivity: −11 days means the imposed date and the logic disagree. And float on a five-day calendar is not float in the world. Six working days across a weekend is eight calendar days.

**Post 7/7 — float is only as honest as the logic** (199 characters)
Open ends and missing predecessors manufacture float that does not exist. Check the logic before you trade the days away.
https://projectcontrolsinstitute.org/total-float
#Scheduling #ProjectControls

---

*Internal links: the final post carries the only link and points at [what total float is](https://projectcontrolsinstitute.org/total-float) with that anchor. Reply posts should use [the critical path method](https://projectcontrolsinstitute.org/critical-path-method) and [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with those anchors.*
