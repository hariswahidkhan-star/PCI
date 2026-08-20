---
platform:      LinkedIn post
type:          linkedin-post
title:         Hard vs soft constraints in P6: one of them lies to you
meta:          A mandatory finish absorbed ten days of slip and reported the project on time. Soft and hard constraints compared, and the three rules worth enforcing.
primary_kw:    P6 constraints
secondary_kw:  mandatory finish, finish on or before, start on or after, schedule quality
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    409
hashtags:      #Scheduling #Primavera #ProjectControls #ProjectManagement
ab_id:         AB-00183
---

# Hard vs soft constraints in P6: one of them lies to you

**Post body (1,899 characters):**

Two schedules, same logic, same durations, same progress. One reports the project on time. One reports ten days late. The only difference is a constraint type.

Soft constraints bend to logic. A "start on or after" pushes the early dates and leaves the network intact. A "finish on or before" pulls the late dates, and where the logic cannot meet it you get negative float. Both are honest: they change the answer and show you they did.

Hard constraints override logic in both directions. A mandatory start or finish holds its date whether or not the predecessors support it, and the slip that should have shown as negative float simply vanishes.

How ten days disappear:

Activity A finishes day 60
Activity B is finish-to-start from A, duration 30, so B runs day 61 to day 90
Someone applies a mandatory finish of day 80 to B
P6 holds day 80. B now reports a finish it cannot reach with a 30-day duration
Activity C reads day 81 and the project reports on time

Those ten days did not go anywhere. They were absorbed by a date somebody typed. Remove the mandatory constraint, reschedule, and the same file shows −10 days total float along the chain — which was true the whole time.

Three rules worth enforcing on any schedule you have to defend.

Count them. Divide constrained activities by total activities. On a 2,400-activity programme, 180 constrained is 7.5%: a schedule steered by hand, not driven by logic. Schedule quality assessments such as the DCMA 14-point review count hard constraints for this reason.

Zero mandatory constraints. If a date genuinely cannot move, model it as a milestone with "finish on or before" and let the negative float show. That is the same information, told truthfully.

Every remaining constraint carries a written reason in the notebook field. A constraint nobody can explain is one nobody dares remove, and it will still be there three baselines later.

#Scheduling #Primavera #ProjectControls #ProjectManagement

**First comment:** Building a schedule that holds its dates through logic rather than constraints, and what to check before baselining: https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6

---

*Every figure above is illustrative arithmetic, not project data. The DCMA 14-point assessment is named and described here in PCI's own words; no protected text or table is reproduced. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with the anchor "modelling dates with logic instead of constraints", and [total float](https://projectcontrolsinstitute.org/total-float) with the anchor "what negative float is telling you".*
