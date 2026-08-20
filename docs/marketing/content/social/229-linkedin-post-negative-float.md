---
platform:      LinkedIn post
type:          linkedin-post
title:         Negative float is about your constraints, not your team
meta:          A 120-day chain against a day-98 contract date reads −22 on every activity. What negative float actually measures, and the five fixes in honest order.
primary_kw:    negative float
secondary_kw:  total float, imposed finish date, longest path, Primavera P6 constraints
pillar:        Planning and scheduling
credential:    PCL-AI
target_domain: projectcontrolsinstitute.org
canonical:     original
schema:        Article
word_count:    349
hashtags:      #Scheduling #ProjectControls #Primavera #ProjectManagement
ab_id:         AB-00182
---

# Negative float is about your constraints, not your team

**Post body (1,899 characters):**

Negative float is not a sign the team is behind. It is a sign a date has been imposed that the logic cannot reach, and the schedule is telling you by how much.

Total float is the late date minus the early date. It goes negative only when a late date sits earlier than an early date, and that can only happen when something outside the logic sets it: an imposed project finish, a contractual milestone, or a constraint typed onto an activity.

A chain of five activities from the data date: 20 + 35 + 15 + 40 + 10 = 120 days.

Earliest possible finish: day 120
Contract completion, constrained: day 98
Total float across the chain: 98 − 120 = −22 days

Every activity in that chain now reads −22. That does not mean each one is 22 days late. It means the chain is 22 days longer than the window allows, once.

The other thing it changes: the critical path is normally the longest path, but with negative float in the model the driving path is the most negative one. If a second chain reads −6, recovering the −22 chain in full leaves you at −6, not zero. Fix the wrong chain and the headline number does not move at all.

Five steps, in the order that keeps you honest.

Find the constraint. Filter on activities where the primary constraint is not "none". It is usually one activity, put there for a reason that expired.

Ask whether the date is contractual or an assumption typed two baselines ago. Very different problems.

If it is contractual, negative float is a true statement about your forecast. Publish it. A schedule showing −22 days is doing its job; the one showing zero because the constraint was deleted is not.

Only then compress. Overlap, re-sequence, add resource — and price each option, because acceleration is bought, not decided.

Never remove the constraint to make the column go green. That is the one fix that is a lie, and it is the one that ends up in the delay analysis.

#Scheduling #ProjectControls #Primavera #ProjectManagement

**First comment:** What total float is, how it is calculated, and why free float and total float answer different questions: https://projectcontrolsinstitute.org/total-float

---

*Every figure above is illustrative arithmetic, not project data. PCI publishes certification requirements; nothing here is legal, tax or accounting advice.*

*Internal links (first comment and profile featured section): [total float](https://projectcontrolsinstitute.org/total-float) with the anchor "how total float is calculated", and [building a realistic schedule in Primavera P6](https://projectcontrolsinstitute.org/realistic-schedule-in-primavera-p6) with the anchor "modelling dates with logic instead of constraints".*
