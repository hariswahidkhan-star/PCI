#!/usr/bin/env python3
"""
PCI Project Intelligence — Year-1 governed content plan generator.

Deterministically produces backend/content/project-intelligence/year-1/:
  manifest.json, 01-january.json … 12-december.json, reserve.json, YEAR1_CONTENT_INDEX.md

The plan is 420 slots (365 scheduled PI-Y1-D001–D365 + 55 reserve PI-Y1-R001–R055), each carrying
its full taxonomy assignment. The 52 existing bank challenges are mapped onto matching scheduled
slots (status "mapped", audit: conditionally accepted — see docs/pciworld/PROJECT_INTELLIGENCE.md);
the remaining slots are the governed authoring backlog (status "planned").

Every approved distribution table is satisfied EXACTLY; the generator asserts this before writing,
and backend/tests/PCI.Backend.Tests/WorldIntelligenceTests.cs re-verifies the committed files in CI
(including that mapped slots agree with Core/WorldIntelligence.cs — the C# classification is the
source of truth; regenerate with this script after changing it).

Re-running the script is idempotent: same input, byte-identical output.
"""
import json, os, sys, re
from collections import Counter

OUT = os.path.join(os.path.dirname(__file__), "..", "content", "project-intelligence", "year-1")

# ── approved vocabularies (keys must match Core/WorldIntelligence.cs) ─────────────────────────────
TYPES = ["daily_decision", "project_rescue", "risk_room", "schedule_strategy",
         "cost_value", "stakeholder_dilemma", "logic_sequence", "executive_mission"]
TYPE_LABEL = {"daily_decision": "Daily Decision", "project_rescue": "Project Rescue",
              "risk_room": "Risk Room", "schedule_strategy": "Schedule Strategy",
              "cost_value": "Cost & Value", "stakeholder_dilemma": "Stakeholder Dilemma",
              "logic_sequence": "Logic & Sequence", "executive_mission": "Executive Mission"}
DOMAINS = ["integration_governance", "scope_requirements", "schedule_planning", "cost_commercial",
           "risk_uncertainty", "quality_assurance", "resources_leadership",
           "stakeholders_communication", "procurement_contracts", "change_claims_recovery",
           "safety_sustainability", "digital_data_ai"]
LIFECYCLES = ["concept_business_case", "definition_planning", "procurement_mobilization",
              "execution_control", "commissioning_handover", "closeout_lessons"]
SECTORS = ["cross_sector", "construction_infrastructure", "energy_utilities", "technology_digital",
           "manufacturing_industrial", "transport_logistics", "public_sector",
           "healthcare_life_sciences", "climate_sustainability", "professional_services_other"]
INTERACTIONS = ["single_decision", "order_rank", "numeric_calculation", "multi_stage_decision",
                "evidence_diagnosis", "negotiation_communication"]
DIFFS = ["foundation", "practitioner", "advanced", "executive"]
BANDS = ["quick", "standard", "deep", "capstone"]

# ── approved distribution tables (spec §9) ────────────────────────────────────────────────────────
SCHED_TYPE = {"daily_decision": 120, "project_rescue": 50, "risk_room": 43, "schedule_strategy": 43,
              "cost_value": 37, "stakeholder_dilemma": 33, "logic_sequence": 26, "executive_mission": 13}
RESERVE_TYPE = {"daily_decision": 20, "project_rescue": 8, "risk_room": 6, "schedule_strategy": 6,
                "cost_value": 5, "stakeholder_dilemma": 4, "logic_sequence": 4, "executive_mission": 2}
SCHED_DOMAIN = {"integration_governance": 45, "scope_requirements": 32, "schedule_planning": 45,
                "cost_commercial": 42, "risk_uncertainty": 42, "quality_assurance": 26,
                "resources_leadership": 28, "stakeholders_communication": 38,
                "procurement_contracts": 28, "change_claims_recovery": 20,
                "safety_sustainability": 10, "digital_data_ai": 9}
SCHED_DIFF = {"foundation": 105, "practitioner": 150, "advanced": 90, "executive": 20}
SCHED_BAND = {"quick": 150, "standard": 130, "deep": 65, "capstone": 20}
SCHED_INTER = {"single_decision": 140, "order_rank": 45, "numeric_calculation": 65,
               "multi_stage_decision": 55, "evidence_diagnosis": 40, "negotiation_communication": 20}
SCHED_LIFE = {"concept_business_case": 45, "definition_planning": 85, "procurement_mobilization": 45,
              "execution_control": 130, "commissioning_handover": 35, "closeout_lessons": 25}
SCHED_SECTOR = {"cross_sector": 105, "construction_infrastructure": 55, "energy_utilities": 40,
                "technology_digital": 40, "manufacturing_industrial": 30, "transport_logistics": 25,
                "public_sector": 25, "healthcare_life_sciences": 20, "climate_sustainability": 15,
                "professional_services_other": 10}
MONTHS = [  # (file no, name, days, first day-of-year, theme, preferred domains)
    (1, "january", 31, 1, "Decision foundations and governance", ["integration_governance"]),
    (2, "february", 28, 32, "Scope, requirements and stakeholder alignment", ["scope_requirements", "stakeholders_communication"]),
    (3, "march", 31, 60, "Planning, sequencing and critical-path judgment", ["schedule_planning"]),
    (4, "april", 30, 91, "Cost, value, forecasting and commercial awareness", ["cost_commercial"]),
    (5, "may", 31, 121, "Risk, opportunity, uncertainty and contingency", ["risk_uncertainty"]),
    (6, "june", 30, 152, "Procurement, contracts, claims and supplier decisions", ["procurement_contracts"]),
    (7, "july", 31, 182, "Delivery control, change and recovery", ["change_claims_recovery", "cost_commercial"]),
    (8, "august", 31, 213, "Leadership, teams, communication and conflict", ["resources_leadership", "stakeholders_communication"]),
    (9, "september", 30, 244, "Quality, assurance, safety and sustainability", ["quality_assurance", "safety_sustainability"]),
    (10, "october", 31, 274, "Digital delivery, data quality and responsible AI", ["digital_data_ai", "quality_assurance"]),
    (11, "november", 30, 305, "Integrated project controls and executive trade-offs", ["integration_governance", "cost_commercial"]),
    (12, "december", 31, 335, "Handover, closeout, lessons and capstone missions", ["integration_governance", "stakeholders_communication"]),
]
assert sum(m[2] for m in MONTHS) == 365

# ── the 52 mapped bank challenges (mirror of Core/WorldIntelligence.cs Classification + pack data;
#    CI cross-checks the committed JSON against the C# tables, so drift cannot survive review) ─────
# code, title, type, domain, lifecycle, sector, interaction, raw difficulty, est minutes
MAPPED = [
    ("WC-EVM-001", "The metro package is bleeding money — or is it?", "cost_value", "cost_commercial", "execution_control", "transport_logistics", "numeric_calculation", "foundation", 8),
    ("WC-SCH-002", "Find the path that actually matters", "schedule_strategy", "schedule_planning", "definition_planning", "technology_digital", "numeric_calculation", "developing", 10),
    ("WC-RSK-003", "Price the storm before it hits", "risk_room", "risk_uncertainty", "definition_planning", "climate_sustainability", "numeric_calculation", "professional", 10),
    ("WC-CSH-004", "The project that ran out of cash while profitable", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation", "professional", 10),
    ("WC-CHG-005", "The scope wants to grow. The budget doesn't.", "daily_decision", "change_claims_recovery", "execution_control", "healthcare_life_sciences", "single_decision", "developing", 8),
    ("WC-PRG-006", "Percent complete, or percent hoped?", "schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation", "foundation", 7),
    ("WC-ESC-007", "The schedule says fine. Time says otherwise.", "schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "numeric_calculation", "advanced", 12),
    ("WC-PRT-008", "Three estimates, one promise", "risk_room", "risk_uncertainty", "definition_planning", "transport_logistics", "numeric_calculation", "professional", 10),
    ("WC-TML-009", "Read the trend, not the month", "schedule_strategy", "schedule_planning", "execution_control", "public_sector", "numeric_calculation", "advanced", 12),
    ("WC-AIA-010", "The AI says relax. The data says don't.", "daily_decision", "digital_data_ai", "execution_control", "technology_digital", "evidence_diagnosis", "expert", 15),
    ("WC-PRD-011", "The crew is working hard. Is the work working?", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation", "foundation", 8),
    ("WC-BOQ-012", "Price the bill before you sign it", "cost_value", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "numeric_calculation", "foundation", 7),
    ("WC-RES-013", "Five weeks, one crew, too much work", "schedule_strategy", "resources_leadership", "definition_planning", "energy_utilities", "numeric_calculation", "developing", 9),
    ("WC-PRC-014", "The crane is late. Is the project?", "daily_decision", "procurement_contracts", "procurement_mobilization", "transport_logistics", "single_decision", "developing", 8),
    ("WC-PTF-015", "Five projects, one envelope", "executive_mission", "integration_governance", "concept_business_case", "cross_sector", "multi_stage_decision", "professional", 12),
    ("WC-DEC-016", "Three ways up the mountain", "daily_decision", "integration_governance", "concept_business_case", "manufacturing_industrial", "single_decision", "professional", 10),
    ("WC-DQA-017", "Garbage in, forecast out", "daily_decision", "digital_data_ai", "execution_control", "technology_digital", "evidence_diagnosis", "professional", 10),
    ("WC-EVM-018", "Turnaround truth at 3 a.m.", "cost_value", "cost_commercial", "execution_control", "energy_utilities", "numeric_calculation", "foundation", 8),
    ("WC-CSH-019", "The show opens in five months", "cost_value", "cost_commercial", "execution_control", "professional_services_other", "numeric_calculation", "developing", 9),
    ("WC-RSK-020", "The spillway question", "risk_room", "risk_uncertainty", "definition_planning", "energy_utilities", "numeric_calculation", "foundation", 8),
    ("WC-PRT-021", "The integration window", "risk_room", "risk_uncertainty", "definition_planning", "public_sector", "numeric_calculation", "advanced", 12),
    ("WC-CPM-022", "The lab that opens in seventeen days — or doesn't", "schedule_strategy", "schedule_planning", "definition_planning", "healthcare_life_sciences", "numeric_calculation", "developing", 9),
    ("WC-CHG-023", "The campus that grew in committee", "daily_decision", "change_claims_recovery", "execution_control", "public_sector", "single_decision", "foundation", 8),
    ("WC-ESC-024", "Ten sprints, one deadline, and a curve that doesn't lie", "schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "numeric_calculation", "advanced", 12),
    ("WC-TML-025", "Five months of drift", "schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "numeric_calculation", "professional", 11),
    ("WC-WBS-026", "Every dollar needs an address", "logic_sequence", "scope_requirements", "definition_planning", "manufacturing_industrial", "numeric_calculation", "foundation", 7),
    ("WC-CBS-027", "Where the depot money went", "cost_value", "cost_commercial", "execution_control", "transport_logistics", "numeric_calculation", "developing", 8),
    ("WC-PRG-028", "How finished is the ship?", "schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation", "foundation", 7),
    ("WC-AIA-029", "The optimistic algorithm", "daily_decision", "digital_data_ai", "execution_control", "technology_digital", "evidence_diagnosis", "advanced", 14),
    ("WC-CAP-030", "The wind farm at the crossroads", "executive_mission", "integration_governance", "concept_business_case", "climate_sustainability", "multi_stage_decision", "expert", 18),
    ("WC-EVM-031", "The data hall that ran out of runway", "cost_value", "cost_commercial", "execution_control", "technology_digital", "numeric_calculation", "advanced", 11),
    ("WC-CPM-032", "Cold chain, hot deadline", "schedule_strategy", "schedule_planning", "definition_planning", "healthcare_life_sciences", "numeric_calculation", "professional", 10),
    ("WC-RSK-033", "The tie-in nobody priced", "risk_room", "risk_uncertainty", "definition_planning", "energy_utilities", "numeric_calculation", "professional", 9),
    ("WC-CSH-034", "The month the money ran out", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation", "developing", 8),
    ("WC-BOQ-035", "Twelve kilometres of earth", "cost_value", "procurement_contracts", "procurement_mobilization", "transport_logistics", "numeric_calculation", "foundation", 6),
    ("WC-PRD-036", "The lining crew that looked fast", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "numeric_calculation", "developing", 8),
    ("WC-RES-037", "The shutdown that needed two of everyone", "schedule_strategy", "resources_leadership", "definition_planning", "energy_utilities", "numeric_calculation", "professional", 9),
    ("WC-CBS-038", "Where the ward money went", "cost_value", "cost_commercial", "execution_control", "healthcare_life_sciences", "numeric_calculation", "developing", 8),
    ("WC-PRT-039", "The launch window will not wait", "risk_room", "risk_uncertainty", "definition_planning", "manufacturing_industrial", "numeric_calculation", "professional", 10),
    ("WC-CHG-040", "Four changes and a baseline", "daily_decision", "change_claims_recovery", "execution_control", "energy_utilities", "single_decision", "developing", 7),
    ("WC-ESC-041", "The fab that was behind in time, not money", "schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation", "advanced", 11),
    ("WC-TML-042", "Six periods, one bad month", "schedule_strategy", "schedule_planning", "execution_control", "manufacturing_industrial", "numeric_calculation", "advanced", 11),
    ("WC-PFO-043", "Three good projects, one budget", "executive_mission", "integration_governance", "concept_business_case", "public_sector", "multi_stage_decision", "expert", 12),
    ("WC-DEC-044", "Haul road, three ways", "daily_decision", "integration_governance", "concept_business_case", "manufacturing_industrial", "single_decision", "professional", 9),
    ("WC-DQ-045", "The dashboard that was confidently wrong", "daily_decision", "digital_data_ai", "execution_control", "transport_logistics", "evidence_diagnosis", "developing", 8),
    ("WC-PRC-046", "The valve that was late enough to matter", "daily_decision", "procurement_contracts", "procurement_mobilization", "manufacturing_industrial", "single_decision", "foundation", 6),
    ("WC-PRG-047", "How much of the campus is finished?", "schedule_strategy", "schedule_planning", "execution_control", "public_sector", "numeric_calculation", "foundation", 6),
    ("WC-WBS-048", "The parent that did not add up", "logic_sequence", "scope_requirements", "definition_planning", "technology_digital", "numeric_calculation", "developing", 7),
    ("WC-EVM-049", "Three forecasts, one reactor", "cost_value", "cost_commercial", "execution_control", "energy_utilities", "numeric_calculation", "expert", 13),
    ("WC-CPM-050", "The terminal that had two ways to be late", "schedule_strategy", "schedule_planning", "definition_planning", "transport_logistics", "numeric_calculation", "expert", 12),
    ("WC-TRC-051", "Walk me back to the signed number", "daily_decision", "quality_assurance", "execution_control", "professional_services_other", "evidence_diagnosis", "developing", 9),
    ("WC-TRC-052", "Every number in this pack has a source — prove it", "daily_decision", "quality_assurance", "execution_control", "healthcare_life_sciences", "evidence_diagnosis", "professional", 10),
]
assert len(MAPPED) == 52

# ── January authored pack (Data/WorldIntelligencePack.cs): items authored TO their plan slot and
#    therefore PINNED to a specific day. Same shape as MAPPED plus the day-of-year. ──
MAPPED_JAN = [
    ("WC-GOV-053", "The gate you cannot half-pass", "daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision", "foundation", 6, 1),
    ("WC-STK-064", "The update that landed badly", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "public_sector", "negotiation_communication", "professional", 7, 2),
    ("WC-RSK-072", "The register nobody opened", "risk_room", "risk_uncertainty", "concept_business_case", "cross_sector", "numeric_calculation", "professional", 9, 3),
    ("WC-CPM-078", "The float that was already spent", "schedule_strategy", "schedule_planning", "concept_business_case", "construction_infrastructure", "numeric_calculation", "professional", 12, 4),
    ("WC-EVM-080", "The pilot that flattered the case", "cost_value", "cost_commercial", "concept_business_case", "cross_sector", "numeric_calculation", "foundation", 8, 5),
    ("WC-GOV-054", "The benefits slide nobody measured", "daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision", "foundation", 7, 6),
    ("WC-STK-065", "Forty residents, one unanswerable question", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "transport_logistics", "negotiation_communication", "professional", 5, 7),
    ("WC-RSK-073", "Drawdown or discipline", "risk_room", "risk_uncertainty", "concept_business_case", "energy_utilities", "numeric_calculation", "professional", 10, 8),
    ("WC-CPM-079", "Two paths, both critical", "schedule_strategy", "schedule_planning", "concept_business_case", "manufacturing_industrial", "numeric_calculation", "professional", 10, 9),
    ("WC-GOV-055", "Two mandates, one road", "daily_decision", "integration_governance", "concept_business_case", "construction_infrastructure", "single_decision", "foundation", 5, 10),
    ("WC-STK-066", "The question you knew was coming", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "construction_infrastructure", "negotiation_communication", "professional", 6, 11),
    ("WC-RSK-074", "The delay that pays", "risk_room", "risk_uncertainty", "concept_business_case", "healthcare_life_sciences", "numeric_calculation", "professional", 8, 12),
    ("WC-GOV-056", "The case that kept growing", "daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision", "foundation", 6, 13),
    ("WC-STK-067", "The email you should not send", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "technology_digital", "negotiation_communication", "professional", 7, 14),
    ("WC-RSK-075", "Five greens and a cliff", "risk_room", "risk_uncertainty", "concept_business_case", "climate_sustainability", "numeric_calculation", "professional", 11, 18),
    ("WC-GOV-057", "Sign here, or send it up?", "daily_decision", "integration_governance", "concept_business_case", "energy_utilities", "single_decision", "foundation", 7, 19),
    ("WC-STK-068", "One pack, two rooms", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "cross_sector", "negotiation_communication", "professional", 5, 20),
    ("WC-RSK-076", "Everyone knew about the gantry", "risk_room", "risk_uncertainty", "concept_business_case", "cross_sector", "numeric_calculation", "professional", 9, 21),
    ("WC-GOV-058", "The baseline that skipped the board", "daily_decision", "integration_governance", "concept_business_case", "technology_digital", "single_decision", "foundation", 5, 22),
    ("WC-STK-069", "Say it first, say it whole", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "cross_sector", "negotiation_communication", "professional", 6, 23),
    ("WC-RSK-077", "Accepted, says who", "risk_room", "risk_uncertainty", "concept_business_case", "technology_digital", "numeric_calculation", "professional", 11, 24),
    ("WC-GOV-059", "Yes to the strategy, no to the start date", "daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision", "foundation", 6, 25),
    ("WC-STK-070", "The partner who heard it in the market", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "energy_utilities", "negotiation_communication", "professional", 7, 26),
    ("WC-GOV-060", "Go, no-go, or not yet", "daily_decision", "integration_governance", "concept_business_case", "construction_infrastructure", "single_decision", "foundation", 7, 27),
    ("WC-STK-071", "The corridor promise", "stakeholder_dilemma", "stakeholders_communication", "concept_business_case", "construction_infrastructure", "negotiation_communication", "professional", 5, 28),
    ("WC-GOV-061", "The gate review with one author", "daily_decision", "integration_governance", "concept_business_case", "manufacturing_industrial", "single_decision", "foundation", 5, 29),
    ("WC-GOV-062", "Benefits now, capability later", "daily_decision", "integration_governance", "concept_business_case", "cross_sector", "single_decision", "foundation", 6, 30),
    ("WC-GOV-063", "One estate, two ministries", "daily_decision", "integration_governance", "concept_business_case", "public_sector", "single_decision", "foundation", 6, 31),
]
assert len(MAPPED_JAN) == 28

# ── February authored pack: scope, requirements and stakeholder alignment. ──
MAPPED_FEB = [
    ("WC-SCO-081", "The requirement nobody owned", "logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank", "foundation", 6, 32),
    ("WC-STK-090", "The workshop that would not converge", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "cross_sector", "negotiation_communication", "professional", 6, 33),
    ("WC-EVM-099", "Three letters the board reads first", "cost_value", "cost_commercial", "definition_planning", "cross_sector", "numeric_calculation", "foundation", 10, 34),
    ("WC-CAP-101", "The definition gateway", "executive_mission", "integration_governance", "definition_planning", "cross_sector", "multi_stage_decision", "expert", 24, 35),
    ("WC-RSK-103", "The register from one workshop", "risk_room", "risk_uncertainty", "definition_planning", "construction_infrastructure", "numeric_calculation", "professional", 8, 36),
    ("WC-CPM-105", "The enabling works nobody scheduled", "schedule_strategy", "schedule_planning", "definition_planning", "climate_sustainability", "numeric_calculation", "professional", 9, 37),
    ("WC-SCO-082", "One deliverable, three definitions", "logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank", "foundation", 7, 38),
    ("WC-STK-091", "Two departments, one front door", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "public_sector", "negotiation_communication", "professional", 7, 39),
    ("WC-CHG-100", "Where the contingency actually went", "cost_value", "cost_commercial", "definition_planning", "manufacturing_industrial", "numeric_calculation", "foundation", 12, 40),
    ("WC-CAP-102", "Seven platforms, one future", "executive_mission", "integration_governance", "definition_planning", "technology_digital", "multi_stage_decision", "expert", 22, 41),
    ("WC-RSK-104", "Contingency before the case closes", "risk_room", "risk_uncertainty", "definition_planning", "cross_sector", "numeric_calculation", "professional", 9, 42),
    ("WC-CPM-106", "Planning the turnaround backwards", "schedule_strategy", "schedule_planning", "definition_planning", "energy_utilities", "numeric_calculation", "professional", 11, 43),
    ("WC-SCO-083", "In scope, out of scope, says who", "logic_sequence", "scope_requirements", "definition_planning", "construction_infrastructure", "order_rank", "foundation", 5, 44),
    ("WC-STK-092", "The regulator reads it differently", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "transport_logistics", "negotiation_communication", "professional", 5, 47),
    ("WC-SCO-084", "Where the project ends and the asset begins", "logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank", "foundation", 6, 48),
    ("WC-STK-093", "Just a clarification", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "cross_sector", "negotiation_communication", "professional", 6, 49),
    ("WC-SCO-085", "The exclusion that came back", "logic_sequence", "scope_requirements", "definition_planning", "energy_utilities", "order_rank", "foundation", 7, 50),
    ("WC-STK-094", "The design review that became a duel", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "construction_infrastructure", "negotiation_communication", "professional", 7, 51),
    ("WC-SCO-086", "Acceptance criteria, after the fact", "logic_sequence", "scope_requirements", "definition_planning", "technology_digital", "order_rank", "foundation", 5, 52),
    ("WC-STK-095", "The must-have that isn't in scope", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "energy_utilities", "negotiation_communication", "professional", 5, 53),
    ("WC-SCO-087", "The backlog that ate the baseline", "logic_sequence", "scope_requirements", "definition_planning", "cross_sector", "order_rank", "foundation", 6, 54),
    ("WC-STK-096", "Whose signature accepts the ward", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "healthcare_life_sciences", "negotiation_communication", "professional", 6, 55),
    ("WC-SCO-088", "Roll it up before you promise it", "logic_sequence", "scope_requirements", "definition_planning", "construction_infrastructure", "order_rank", "foundation", 7, 56),
    ("WC-STK-097", "The demo that sold what we didn't spec", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "technology_digital", "negotiation_communication", "professional", 7, 57),
    ("WC-SCO-089", "The spare-parts scope nobody claimed", "logic_sequence", "scope_requirements", "definition_planning", "manufacturing_industrial", "order_rank", "foundation", 5, 58),
    ("WC-STK-098", "The requirement announced to the press", "stakeholder_dilemma", "stakeholders_communication", "definition_planning", "cross_sector", "negotiation_communication", "professional", 5, 59),
]
assert len(MAPPED_FEB) == 26

# ── March authored pack: planning, sequencing and critical-path judgment. ──
MAPPED_MAR = [
    ("WC-SCH-107", "Float is a budget, not a rumour", "schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation", "professional", 9, 60),
    ("WC-CBS-116", "Where the estate money stands", "cost_value", "cost_commercial", "definition_planning", "public_sector", "numeric_calculation", "foundation", 12, 61),
    ("WC-RSK-119", "The register meets the review board", "risk_room", "risk_uncertainty", "definition_planning", "cross_sector", "numeric_calculation", "professional", 9, 62),
    ("WC-CAP-122", "The corridor decade", "executive_mission", "integration_governance", "definition_planning", "transport_logistics", "multi_stage_decision", "expert", 24, 63),
    ("WC-STK-124", "Two projects, one principal engineer", "stakeholder_dilemma", "resources_leadership", "definition_planning", "cross_sector", "negotiation_communication", "professional", 6, 64),
    ("WC-SCH-108", "The shorter path the client watches", "schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation", "professional", 10, 65),
    ("WC-CSH-117", "The programme that must borrow to breathe", "cost_value", "cost_commercial", "definition_planning", "cross_sector", "numeric_calculation", "foundation", 10, 66),
    ("WC-RSK-120", "Drawdown at definition", "risk_room", "risk_uncertainty", "definition_planning", "construction_infrastructure", "numeric_calculation", "professional", 8, 67),
    ("WC-CAP-123", "The outage portfolio", "executive_mission", "integration_governance", "definition_planning", "energy_utilities", "multi_stage_decision", "expert", 22, 68),
    ("WC-SCH-109", "A milestone without a parent", "schedule_strategy", "schedule_planning", "definition_planning", "construction_infrastructure", "numeric_calculation", "professional", 8, 82),
    ("WC-BOQ-118", "Price the alternative before you praise it", "cost_value", "cost_commercial", "definition_planning", "healthcare_life_sciences", "numeric_calculation", "foundation", 11, 83),
    ("WC-RSK-121", "Three documents, one real risk", "risk_room", "risk_uncertainty", "definition_planning", "technology_digital", "evidence_diagnosis", "professional", 12, 84),
    ("WC-SCH-110", "The recovery that raided the test window", "schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation", "professional", 11, 85),
    ("WC-SCH-111", "Do the ugly work first", "schedule_strategy", "schedule_planning", "definition_planning", "energy_utilities", "numeric_calculation", "professional", 12, 86),
    ("WC-SCH-112", "Three estimates for one calendar", "schedule_strategy", "schedule_planning", "definition_planning", "technology_digital", "numeric_calculation", "professional", 9, 87),
    ("WC-SCH-113", "The trend already voted", "schedule_strategy", "schedule_planning", "definition_planning", "cross_sector", "numeric_calculation", "professional", 10, 88),
    ("WC-SCH-114", "The handover date, third edition", "schedule_strategy", "schedule_planning", "definition_planning", "construction_infrastructure", "numeric_calculation", "professional", 8, 89),
    ("WC-SCH-115", "Eighty percent done, twice", "schedule_strategy", "schedule_planning", "definition_planning", "manufacturing_industrial", "numeric_calculation", "professional", 11, 90),
]
assert len(MAPPED_MAR) == 18

# ── April authored pack: cost, value, forecasting and commercial awareness. ──
MAPPED_APR = [
    ("WC-CST-125", "The savings that existed only in the forecast", "cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision", "foundation", 9, 91),
    ("WC-QLT-135", "Signed, sealed, not inspected", "daily_decision", "quality_assurance", "execution_control", "healthcare_life_sciences", "single_decision", "foundation", 7, 92),
    ("WC-RSK-141", "The register says calm. The depot says otherwise.", "risk_room", "risk_uncertainty", "execution_control", "transport_logistics", "evidence_diagnosis", "professional", 9, 93),
    ("WC-STK-145", "One engineer, two emergencies", "stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "negotiation_communication", "professional", 6, 94),
    ("WC-CAP-148", "The quarter everything slipped", "executive_mission", "integration_governance", "execution_control", "cross_sector", "multi_stage_decision", "expert", 24, 95),
    ("WC-CST-126", "The contingency that bled quietly", "cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision", "foundation", 10, 96),
    ("WC-PRC-136", "Forty percent below the next bid", "daily_decision", "procurement_contracts", "execution_control", "climate_sustainability", "single_decision", "foundation", 6, 97),
    ("WC-RSK-142", "The drawdown for the wrong risk", "risk_room", "risk_uncertainty", "execution_control", "energy_utilities", "evidence_diagnosis", "professional", 10, 98),
    ("WC-STK-146", "The implementation team that exists twice", "stakeholder_dilemma", "resources_leadership", "execution_control", "technology_digital", "negotiation_communication", "professional", 5, 99),
    ("WC-CAP-149", "Ten percent less, same promise", "executive_mission", "integration_governance", "execution_control", "construction_infrastructure", "multi_stage_decision", "expert", 22, 100),
    ("WC-CST-127", "The cheaper panel arrives tomorrow", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "single_decision", "foundation", 8, 101),
    ("WC-GOV-137", "The new sponsor wants a new baseline", "daily_decision", "integration_governance", "execution_control", "cross_sector", "single_decision", "foundation", 7, 102),
    ("WC-RSK-143", "Twelve weeks late, one door open", "risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis", "professional", 8, 103),
    ("WC-STK-147", "Take the keys, or leave them", "stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "single_decision", "professional", 6, 109),
    ("WC-CST-128", "The month the curve crossed the line", "cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision", "foundation", 11, 110),
    ("WC-QLT-138", "The snag that wasn't a snag", "daily_decision", "quality_assurance", "execution_control", "construction_infrastructure", "single_decision", "foundation", 5, 111),
    ("WC-RSK-144", "The green map and the red festival", "risk_room", "risk_uncertainty", "execution_control", "professional_services_other", "evidence_diagnosis", "professional", 11, 112),
    ("WC-CST-129", "Rates from another market", "cost_value", "cost_commercial", "execution_control", "energy_utilities", "single_decision", "foundation", 12, 113),
    ("WC-PRC-139", "Priced at midnight, mid-outage", "daily_decision", "procurement_contracts", "execution_control", "energy_utilities", "single_decision", "foundation", 7, 114),
    ("WC-CST-130", "The ledger nobody reconciled", "cost_value", "cost_commercial", "execution_control", "technology_digital", "single_decision", "foundation", 9, 115),
    ("WC-GOV-140", "Fifteen percent bigger, same business case", "daily_decision", "integration_governance", "execution_control", "manufacturing_industrial", "single_decision", "foundation", 5, 116),
    ("WC-CST-131", "One project, three estimates at completion", "cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision", "foundation", 10, 117),
    ("WC-CST-132", "Small yeses, thin margin", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "single_decision", "foundation", 8, 118),
    ("WC-CST-133", "The milestone that was paid early", "cost_value", "cost_commercial", "execution_control", "manufacturing_industrial", "single_decision", "foundation", 11, 119),
    ("WC-CST-134", "Spend it by March", "cost_value", "cost_commercial", "execution_control", "public_sector", "single_decision", "foundation", 12, 120),
]
assert len(MAPPED_APR) == 25

# ── May authored pack: risk, opportunity, uncertainty and contingency. ──
MAPPED_MAY = [
    ("WC-RSK-150", "Four warranty claims, one pattern", "risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis", "professional", 9, 121),
    ("WC-QLT-160", "The survey kit with the expired sticker", "daily_decision", "quality_assurance", "execution_control", "transport_logistics", "single_decision", "foundation", 6, 122),
    ("WC-STK-166", "The team that stopped saying no", "stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "single_decision", "professional", 7, 123),
    ("WC-CAP-171", "The appetite reset", "executive_mission", "integration_governance", "execution_control", "cross_sector", "multi_stage_decision", "expert", 24, 124),
    ("WC-SCO-173", "The emergency that rewrote the scope", "logic_sequence", "scope_requirements", "execution_control", "energy_utilities", "order_rank", "foundation", 6, 125),
    ("WC-RSK-151", "The trend and the threshold", "risk_room", "risk_uncertainty", "execution_control", "construction_infrastructure", "evidence_diagnosis", "professional", 10, 126),
    ("WC-PRC-163", "Working on a letter, waiting on a contract", "daily_decision", "procurement_contracts", "execution_control", "construction_infrastructure", "single_decision", "foundation", 5, 127),
    ("WC-STK-167", "The counteroffer conversation", "stakeholder_dilemma", "resources_leadership", "execution_control", "technology_digital", "single_decision", "professional", 5, 128),
    ("WC-CAP-172", "The incident on someone else's project", "executive_mission", "integration_governance", "execution_control", "construction_infrastructure", "multi_stage_decision", "expert", 22, 129),
    ("WC-SCO-174", "Thirty requests, one intake", "logic_sequence", "scope_requirements", "execution_control", "cross_sector", "order_rank", "foundation", 5, 130),
    ("WC-RSK-152", "The month nobody nearly got hurt", "risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis", "professional", 8, 131),
    ("WC-QLT-161", "The shortcut through the cleanroom", "daily_decision", "quality_assurance", "execution_control", "healthcare_life_sciences", "single_decision", "foundation", 7, 132),
    ("WC-STK-168", "The map that went viral", "stakeholder_dilemma", "stakeholders_communication", "execution_control", "climate_sustainability", "single_decision", "professional", 6, 133),
    ("WC-RSK-153", "The register the leaver left behind", "risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis", "professional", 11, 140),
    ("WC-PRC-164", "One quote, familiar face", "daily_decision", "procurement_contracts", "execution_control", "cross_sector", "single_decision", "foundation", 7, 141),
    ("WC-STK-169", "The secondee's other employer", "stakeholder_dilemma", "resources_leadership", "execution_control", "public_sector", "single_decision", "professional", 7, 142),
    ("WC-RSK-154", "The supplier's quiet quarter", "risk_room", "risk_uncertainty", "execution_control", "energy_utilities", "evidence_diagnosis", "professional", 12, 143),
    ("WC-QLT-162", "The finding that got friendlier", "daily_decision", "quality_assurance", "execution_control", "cross_sector", "single_decision", "foundation", 6, 144),
    ("WC-STK-170", "Forty-eight hours of darkness", "stakeholder_dilemma", "stakeholders_communication", "execution_control", "energy_utilities", "single_decision", "professional", 5, 145),
    ("WC-RSK-155", "Seventeen findings and a launch date", "risk_room", "risk_uncertainty", "execution_control", "technology_digital", "evidence_diagnosis", "professional", 9, 146),
    ("WC-PRC-165", "The part that changed inside the box", "daily_decision", "procurement_contracts", "execution_control", "manufacturing_industrial", "single_decision", "foundation", 6, 147),
    ("WC-RSK-156", "Ninety-six percent of the chart", "risk_room", "risk_uncertainty", "execution_control", "construction_infrastructure", "evidence_diagnosis", "professional", 10, 148),
    ("WC-RSK-157", "The exclusion in the renewal", "risk_room", "risk_uncertainty", "execution_control", "cross_sector", "evidence_diagnosis", "professional", 8, 149),
    ("WC-RSK-158", "The milestone that was true from a distance", "risk_room", "risk_uncertainty", "execution_control", "public_sector", "evidence_diagnosis", "professional", 11, 150),
    ("WC-RSK-159", "One rig, three lines", "risk_room", "risk_uncertainty", "execution_control", "manufacturing_industrial", "evidence_diagnosis", "professional", 12, 151),
]
assert len(MAPPED_MAY) == 25

# ── June authored pack: procurement, contracts, claims and supplier decisions. ──
MAPPED_JUN = [
    ("WC-PRC-175", "The answer that changed the question", "daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision", "foundation", 6, 152),
    ("WC-STK-195", "The mobilization team that hasn't finished demobilizing", "stakeholder_dilemma", "resources_leadership", "procurement_mobilization", "cross_sector", "single_decision", "professional", 6, 153),
    ("WC-SCO-189", "Freeze it before you price it", "logic_sequence", "scope_requirements", "procurement_mobilization", "cross_sector", "order_rank", "foundation", 7, 154),
    ("WC-SCH-192", "Order the orders", "schedule_strategy", "schedule_planning", "procurement_mobilization", "construction_infrastructure", "order_rank", "professional", 9, 155),
    ("WC-CAP-199", "Make, buy, or regret", "executive_mission", "integration_governance", "procurement_mobilization", "manufacturing_industrial", "multi_stage_decision", "expert", 24, 156),
    ("WC-PRC-176", "The bid with the asterisk", "daily_decision", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "single_decision", "foundation", 7, 157),
    ("WC-STK-196", "The village heard it from the winner", "stakeholder_dilemma", "stakeholders_communication", "procurement_mobilization", "energy_utilities", "single_decision", "professional", 7, 158),
    ("WC-SCO-190", "Addendum or answer", "logic_sequence", "scope_requirements", "procurement_mobilization", "climate_sustainability", "order_rank", "foundation", 5, 159),
    ("WC-SCH-193", "First fence, then everything", "schedule_strategy", "schedule_planning", "procurement_mobilization", "cross_sector", "order_rank", "professional", 10, 160),
    ("WC-CAP-200", "The platform and the exit", "executive_mission", "integration_governance", "procurement_mobilization", "technology_digital", "multi_stage_decision", "expert", 22, 161),
    ("WC-PRC-177", "Lunch with the incumbent", "daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision", "foundation", 5, 162),
    ("WC-STK-197", "Two tenders, one bid manager", "stakeholder_dilemma", "resources_leadership", "procurement_mobilization", "cross_sector", "single_decision", "professional", 7, 163),
    ("WC-SCO-191", "Ready is a checklist, not a feeling", "logic_sequence", "scope_requirements", "procurement_mobilization", "public_sector", "order_rank", "foundation", 7, 164),
    ("WC-SCH-194", "The possession chain", "schedule_strategy", "schedule_planning", "procurement_mobilization", "transport_logistics", "order_rank", "professional", 8, 169),
    ("WC-PRC-178", "Ninety days, ninety-one", "daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision", "foundation", 6, 170),
    ("WC-STK-198", "The local firm and the local paper", "stakeholder_dilemma", "stakeholders_communication", "procurement_mobilization", "professional_services_other", "single_decision", "professional", 5, 171),
    ("WC-PRC-179", "Money before metal", "daily_decision", "procurement_contracts", "procurement_mobilization", "energy_utilities", "single_decision", "foundation", 7, 172),
    ("WC-PRC-180", "Clause 14.3, quietly evergreen", "daily_decision", "procurement_contracts", "procurement_mobilization", "technology_digital", "single_decision", "foundation", 5, 173),
    ("WC-PRC-181", "Start Monday, sign eventually", "daily_decision", "procurement_contracts", "procurement_mobilization", "construction_infrastructure", "single_decision", "foundation", 6, 174),
    ("WC-PRC-182", "The debrief that becomes evidence", "daily_decision", "procurement_contracts", "procurement_mobilization", "cross_sector", "single_decision", "foundation", 7, 175),
    ("WC-PRC-183", "Promises by the tonne", "daily_decision", "procurement_contracts", "procurement_mobilization", "public_sector", "single_decision", "foundation", 5, 176),
    ("WC-PRC-184", "The damages nobody believes", "daily_decision", "procurement_contracts", "procurement_mobilization", "transport_logistics", "single_decision", "foundation", 6, 177),
    ("WC-QLT-185", "The certificate that lapsed in transit", "daily_decision", "quality_assurance", "procurement_mobilization", "healthcare_life_sciences", "single_decision", "foundation", 5, 178),
    ("WC-QLT-186", "Approve it from the photos", "daily_decision", "quality_assurance", "procurement_mobilization", "construction_infrastructure", "single_decision", "foundation", 6, 179),
    ("WC-GOV-188", "Award now, fund later", "daily_decision", "integration_governance", "procurement_mobilization", "cross_sector", "single_decision", "foundation", 6, 180),
    ("WC-QLT-187", "Nine welders, six tickets", "daily_decision", "quality_assurance", "procurement_mobilization", "energy_utilities", "single_decision", "foundation", 5, 181),
]
assert len(MAPPED_JUN) == 26

# ── July authored pack: delivery control, change and recovery. ──
MAPPED_JUL = [
    ("WC-RSC-201", "Eight weeks down, one story up", "project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision", "professional", 15, 182),
    ("WC-CST-211", "The index in the invoice", "cost_value", "cost_commercial", "execution_control", "transport_logistics", "single_decision", "foundation", 9, 183),
    ("WC-GOV-219", "Signed above the line", "daily_decision", "integration_governance", "execution_control", "cross_sector", "single_decision", "foundation", 5, 184),
    ("WC-STK-222", "The recovery is working. The people aren't.", "stakeholder_dilemma", "resources_leadership", "execution_control", "cross_sector", "single_decision", "professional", 6, 185),
    ("WC-SCO-225", "Build the claim file backwards", "logic_sequence", "scope_requirements", "execution_control", "professional_services_other", "order_rank", "foundation", 6, 186),
    ("WC-SCH-226", "Rank the rescues", "schedule_strategy", "schedule_planning", "execution_control", "technology_digital", "order_rank", "professional", 11, 187),
    ("WC-RSC-202", "The wall that moved", "project_rescue", "change_claims_recovery", "execution_control", "construction_infrastructure", "multi_stage_decision", "professional", 13, 188),
    ("WC-CST-212", "The quarter ends Friday. The invoices don't.", "cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision", "foundation", 10, 189),
    ("WC-QLT-220", "Severity two, says who", "daily_decision", "quality_assurance", "execution_control", "cross_sector", "single_decision", "foundation", 7, 190),
    ("WC-STK-223", "The clerk of works and the site team", "stakeholder_dilemma", "stakeholders_communication", "execution_control", "construction_infrastructure", "single_decision", "professional", 7, 191),
    ("WC-RSC-203", "Sixty percent adopted, one hundred percent spent", "project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision", "professional", 18, 192),
    ("WC-CST-213", "The donor's wing", "cost_value", "cost_commercial", "execution_control", "healthcare_life_sciences", "single_decision", "foundation", 8, 193),
    ("WC-COM-221", "The minister wants a hard hat", "daily_decision", "stakeholders_communication", "execution_control", "public_sector", "single_decision", "foundation", 6, 194),
    ("WC-STK-224", "Two supervisors, one night shift", "stakeholder_dilemma", "resources_leadership", "execution_control", "manufacturing_industrial", "single_decision", "professional", 5, 200),
    ("WC-RSC-204", "Third time, or never", "project_rescue", "change_claims_recovery", "execution_control", "technology_digital", "multi_stage_decision", "professional", 16, 201),
    ("WC-CST-214", "Provisional no longer", "cost_value", "cost_commercial", "execution_control", "construction_infrastructure", "single_decision", "foundation", 11, 202),
    ("WC-RSC-205", "The administrators answer the phone now", "project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision", "professional", 14, 203),
    ("WC-CST-215", "Capitalise the argument", "cost_value", "cost_commercial", "execution_control", "technology_digital", "single_decision", "foundation", 12, 204),
    ("WC-RSC-206", "The fault that ate the outage", "project_rescue", "change_claims_recovery", "execution_control", "energy_utilities", "multi_stage_decision", "professional", 15, 205),
    ("WC-CST-216", "The euro moved. The invoice didn't yet.", "cost_value", "cost_commercial", "execution_control", "cross_sector", "single_decision", "foundation", 9, 206),
    ("WC-RSC-207", "Two metres of water, twelve weeks of plan", "project_rescue", "change_claims_recovery", "execution_control", "construction_infrastructure", "multi_stage_decision", "professional", 13, 207),
    ("WC-CST-217", "Claim it in March, earn it in May", "cost_value", "cost_commercial", "execution_control", "climate_sustainability", "single_decision", "foundation", 10, 208),
    ("WC-RSC-208", "All red at once", "project_rescue", "change_claims_recovery", "execution_control", "cross_sector", "multi_stage_decision", "professional", 18, 209),
    ("WC-CST-218", "Time and a half, every week", "cost_value", "cost_commercial", "execution_control", "energy_utilities", "single_decision", "foundation", 8, 210),
    ("WC-RSC-209", "Sixty percent of nameplate", "project_rescue", "change_claims_recovery", "execution_control", "manufacturing_industrial", "multi_stage_decision", "professional", 16, 211),
    ("WC-RSC-210", "Rollback is also a decision", "project_rescue", "change_claims_recovery", "execution_control", "public_sector", "multi_stage_decision", "professional", 14, 212),
]
assert len(MAPPED_JUL) == 26
MAPPED_JAN = MAPPED_JAN + MAPPED_FEB + MAPPED_MAR + MAPPED_APR + MAPPED_MAY + MAPPED_JUN + MAPPED_JUL

# Existing bank items that complete January, February and March — pinned so the months stay
# fully authored (March's thirteen legacy schedule items hold their theme-month days).
PINNED_EXISTING = {"WC-DEC-044": 15, "WC-PTF-015": 16, "WC-CAP-030": 17,
                   "WC-WBS-048": 45, "WC-WBS-026": 46,
                   "WC-CPM-050": 69, "WC-TML-042": 70, "WC-CPM-032": 71, "WC-TML-025": 72,
                   "WC-CPM-022": 73, "WC-ESC-007": 74, "WC-SCH-002": 75, "WC-PRG-006": 76,
                   "WC-TML-009": 77, "WC-ESC-024": 78, "WC-PRG-028": 79, "WC-ESC-041": 80,
                   "WC-PRG-047": 81,
                   "WC-PRD-036": 104, "WC-EVM-018": 105, "WC-CSH-004": 106, "WC-CBS-027": 107,
                   "WC-EVM-049": 108,
                   "WC-RSK-033": 134, "WC-RSK-020": 135, "WC-RSK-003": 136, "WC-PRT-008": 137,
                   "WC-PRT-021": 138, "WC-PRT-039": 139,
                   "WC-PRC-046": 165, "WC-PRC-014": 166, "WC-BOQ-012": 167, "WC-BOQ-035": 168,
                   "WC-CHG-040": 195, "WC-CHG-023": 196, "WC-EVM-001": 197, "WC-CHG-005": 198,
                   "WC-CSH-034": 199}

DIFF_BAND = {"foundation": "foundation", "developing": "foundation", "professional": "practitioner",
             "advanced": "advanced", "expert": "executive"}
def duration_band(m): return "quick" if m <= 7 else "standard" if m <= 12 else "deep" if m <= 20 else "capstone"

# ── preference tables for coherent planned slots ──────────────────────────────────────────────────
TYPE_FOR_DOMAIN = {  # types that most naturally carry each domain (fallback: daily_decision, then any)
    "integration_governance": ["executive_mission", "daily_decision", "stakeholder_dilemma", "project_rescue"],
    "scope_requirements": ["logic_sequence", "daily_decision", "project_rescue"],
    "schedule_planning": ["schedule_strategy", "logic_sequence", "project_rescue"],
    "cost_commercial": ["cost_value", "daily_decision", "project_rescue"],
    "risk_uncertainty": ["risk_room", "project_rescue", "daily_decision"],
    "quality_assurance": ["daily_decision", "logic_sequence", "project_rescue"],
    "resources_leadership": ["stakeholder_dilemma", "schedule_strategy", "daily_decision"],
    "stakeholders_communication": ["stakeholder_dilemma", "daily_decision", "executive_mission"],
    "procurement_contracts": ["daily_decision", "cost_value", "stakeholder_dilemma", "project_rescue"],
    "change_claims_recovery": ["project_rescue", "daily_decision", "cost_value"],
    "safety_sustainability": ["daily_decision", "risk_room", "stakeholder_dilemma"],
    "digital_data_ai": ["daily_decision", "logic_sequence"],
}
INTER_FOR_TYPE = {
    "daily_decision": ["single_decision", "evidence_diagnosis", "negotiation_communication"],
    "project_rescue": ["multi_stage_decision", "evidence_diagnosis", "single_decision"],
    "risk_room": ["numeric_calculation", "evidence_diagnosis", "order_rank", "single_decision"],
    "schedule_strategy": ["numeric_calculation", "order_rank", "single_decision"],
    "cost_value": ["numeric_calculation", "single_decision", "evidence_diagnosis"],
    "stakeholder_dilemma": ["negotiation_communication", "single_decision", "multi_stage_decision"],
    "logic_sequence": ["order_rank", "evidence_diagnosis", "single_decision"],
    "executive_mission": ["multi_stage_decision", "single_decision"],
}
BAND_FOR_TYPE = {
    "daily_decision": ["quick", "standard"],
    "project_rescue": ["deep", "standard", "capstone"],
    "risk_room": ["standard", "quick", "deep"],
    "schedule_strategy": ["standard", "quick", "deep"],
    "cost_value": ["standard", "quick", "deep"],
    "stakeholder_dilemma": ["quick", "standard", "deep"],
    "logic_sequence": ["quick", "standard"],
    "executive_mission": ["capstone", "deep"],
}
DIFF_FOR_TYPE = {
    "daily_decision": ["foundation", "practitioner", "advanced"],
    "project_rescue": ["practitioner", "advanced", "executive"],
    "risk_room": ["practitioner", "foundation", "advanced"],
    "schedule_strategy": ["practitioner", "foundation", "advanced"],
    "cost_value": ["foundation", "practitioner", "advanced"],
    "stakeholder_dilemma": ["practitioner", "advanced", "foundation"],
    "logic_sequence": ["foundation", "practitioner"],
    "executive_mission": ["executive", "advanced"],
}
LIFE_FOR_MONTH = {1: ["concept_business_case"], 2: ["definition_planning", "concept_business_case"],
                  3: ["definition_planning"], 6: ["procurement_mobilization"],
                  7: ["execution_control"], 9: ["execution_control", "commissioning_handover"],
                  11: ["execution_control"], 12: ["closeout_lessons", "commissioning_handover"]}

TOPICS = {  # working-title phrase banks per domain (no banned terms; professional register)
    "integration_governance": ["The stage gate that split the board", "Benefits on paper, pressure in the room",
        "One sponsor, two mandates", "The programme that outgrew its case", "A delegation question with teeth",
        "The baseline the board never saw", "Strategy says yes, capacity says no", "The go/no-go with missing evidence"],
    "scope_requirements": ["The requirement nobody owned", "A deliverable with three definitions",
        "The scope statement that meant four things", "Where the fit-out ends and the asset begins",
        "The exclusion that came back", "Acceptance criteria, written after the fact", "The backlog that ate the baseline"],
    "schedule_planning": ["The float that was already spent", "Two critical paths, one promise",
        "A milestone with no logic behind it", "The recovery plan that borrowed from testing",
        "Sequencing the work nobody wants first", "The calendar the site never agreed to",
        "A forecast the trend does not support", "The handover date that moved twice"],
    "cost_commercial": ["The forecast that flattered the month", "Contingency, spent in silence",
        "A value engineering call under pressure", "The cash curve that crossed the facility",
        "Unit rates that stopped meaning anything", "The commitment ledger nobody reconciled",
        "An estimate at completion with three authors", "The margin that left in small pieces"],
    "risk_uncertainty": ["The register that stopped being read", "A contingency drawdown decision",
        "The opportunity hidden in the delay", "Exposure the heat map understated",
        "The single point of failure everyone knew", "A residual risk with no owner",
        "The weather window and the reserve", "Quantifying the risk nobody would name"],
    "quality_assurance": ["The inspection that was signed, not done", "A non-conformance on the critical path",
        "The audit trail with a missing week", "Rework the dashboard called progress",
        "The hold point under commercial pressure", "Assurance evidence that proved too little"],
    "resources_leadership": ["Two projects, one senior engineer", "The crew that was promised twice",
        "A handover the team was not ready for", "The specialist who became the bottleneck",
        "Overtime as a plan, again", "The onboarding that never happened", "A team split across three sites"],
    "stakeholders_communication": ["The update that landed badly", "A community meeting with one answer left",
        "The regulator's question you saw coming", "An escalation drafted in anger",
        "The steering pack with two audiences", "Bad news, first telling", "The partner who heard it elsewhere",
        "A commitment made in the corridor"],
    "procurement_contracts": ["The bid that was too good", "A variation priced under duress",
        "The long-lead item with no float", "Liquidated damages, tested", "The supplier who asked for help early",
        "An instruction the contract never defined", "The tender clarification that changed the job"],
    "change_claims_recovery": ["The change that arrived as a fact", "A claim built from diary pages",
        "Re-baselining without losing the story", "The acceleration that cost more than the delay",
        "Scope growth in committee minutes", "The recovery plan with two owners"],
    "safety_sustainability": ["The shortcut the schedule invited", "A near miss the report softened",
        "Embodied carbon versus programme", "The social value promise coming due",
        "A permit condition discovered late", "Safety evidence the client asked to see"],
    "digital_data_ai": ["The model that was confidently wrong", "A dashboard with two sources of truth",
        "The forecast the algorithm inherited", "Data quality as a delivery risk",
        "An AI recommendation without provenance"],
}
SETTINGS = {  # per-sector project settings for working titles
    "cross_sector": ["a multi-site capital programme", "a portfolio review", "a joint-venture delivery office",
                     "a framework programme", "an enterprise transformation"],
    "construction_infrastructure": ["a highway upgrade", "a river crossing", "a mixed-use development",
                                    "a flood defence scheme", "a stadium fit-out"],
    "energy_utilities": ["a grid reinforcement project", "a substation replacement", "a refinery turnaround",
                         "a water treatment upgrade", "an offshore maintenance campaign"],
    "technology_digital": ["a platform migration", "a data-centre build", "a national rollout",
                           "a core-system replacement", "a network modernisation"],
    "manufacturing_industrial": ["a production line install", "a plant expansion", "a factory relocation",
                                 "a pilot production ramp", "an equipment overhaul"],
    "transport_logistics": ["a depot modernisation", "a port expansion", "a rail resignalling project",
                            "an airport baggage upgrade", "a fleet transition"],
    "public_sector": ["a courts modernisation programme", "a schools estate programme", "a defence facility upgrade",
                      "a census-scale IT programme", "a hospital trust capital plan"],
    "healthcare_life_sciences": ["a sterile facility build", "a laboratory relocation", "a hospital ward refurbishment",
                                 "a clinical trials expansion", "a vaccine plant upgrade"],
    "climate_sustainability": ["a solar portfolio build-out", "a retrofit programme", "a wind farm repowering",
                               "a coastal resilience scheme", "an EV charging rollout"],
    "professional_services_other": ["a museum gallery rebuild", "a broadcast facility move", "a festival build",
                                    "a headquarters relocation", "a conference-centre refit"],
}
BANNED = re.compile(r"\b(game|games|gaming|mini-?games?|puzzle|puzzles|trivia|arcade|leaderboard|jackpot|entertainment)\b", re.I)

MINUTES = {"quick": [6, 7, 5], "standard": [9, 10, 8, 11, 12], "deep": [15, 13, 18, 16, 14], "capstone": [24, 22, 26, 28]}


def take(counter, prefs, fallback_pool):
    """Take one unit from the first preferred key with remaining quota; else the max-remaining key."""
    for p in prefs:
        if counter.get(p, 0) > 0:
            counter[p] -= 1
            return p
    k = max((x for x in fallback_pool if counter.get(x, 0) > 0), key=lambda x: counter[x])
    counter[k] -= 1
    return k


def main():
    # remaining quotas after the mapped items consume theirs
    rt, rd = Counter(SCHED_TYPE), Counter(SCHED_DOMAIN)
    rdi, rb = Counter(SCHED_DIFF), Counter(SCHED_BAND)
    ri, rl, rs = Counter(SCHED_INTER), Counter(SCHED_LIFE), Counter(SCHED_SECTOR)
    for (_c, _t, ty, dom, life, sec, inter, diff, mins) in MAPPED + [t[:9] for t in MAPPED_JAN]:
        rt[ty] -= 1; rd[dom] -= 1; rdi[DIFF_BAND[diff]] -= 1; rb[duration_band(mins)] -= 1
        ri[inter] -= 1; rl[life] -= 1; rs[sec] -= 1
    for c in (rt, rd, rdi, rb, ri, rl, rs):
        assert all(v >= 0 for v in c.values()), f"mapped bank exceeds a quota: {c}"

    # place mapped items into theme-matching months, ROTATING among matching months so no single
    # month absorbs a whole domain (three Executive Missions on consecutive weeks is a plan;
    # three in one month is a rut)
    month_free = {m[0]: list(range(m[3], m[3] + m[2])) for m in MONTHS}  # day-of-year lists
    month_domains = {m[0]: m[5] for m in MONTHS}
    month_mapped = Counter()
    placed = {}  # day -> mapped tuple
    # Pinned items first: they own their day outright.
    for t in MAPPED_JAN:
        day = t[9]
        placed[day] = t[:9]
        for no in month_free:
            if day in month_free[no]:
                month_free[no].remove(day); month_mapped[no] += 1
    for item in MAPPED:
        if item[0] in PINNED_EXISTING:
            day = PINNED_EXISTING[item[0]]
            placed[day] = item
            for no in month_free:
                if day in month_free[no]:
                    month_free[no].remove(day); month_mapped[no] += 1
            continue
        dom = item[3]
        pref = [no for no, doms in month_domains.items() if dom in doms and month_free[no]]
        pool = pref if pref else [no for no in month_free if month_free[no]]
        no = min(pool, key=lambda n: (month_mapped[n], -len(month_free[n])))
        month_mapped[no] += 1
        free = month_free[no]
        day = free[len(free) // 2]          # middle of what's left → even spread
        free.remove(day)
        placed[day] = item

    # fill the remaining 313 slots
    entries = {}
    used_titles = set(t for (_c, t, *_rest) in MAPPED) | set(t for (_c, t, *_rest) in MAPPED_JAN)
    topic_idx = Counter(); setting_idx = Counter(); minute_idx = Counter()

    def title_for(dom, sec):
        for _ in range(80):
            topics = TOPICS[dom]; settings = SETTINGS[sec]
            t = topics[topic_idx[dom] % len(topics)]; topic_idx[dom] += 1
            s = settings[setting_idx[sec] % len(settings)]; setting_idx[sec] += 1
            cand = f"{t} — {s}"
            if cand not in used_titles:
                used_titles.add(cand)
                return cand
        raise SystemExit(f"could not find a unique working title for {dom}/{sec}")

    month_dom_count = Counter()  # (month, domain) soft cap so no month becomes one topic
    sector_quota = {s2: rs[s2] for s2 in SECTORS}  # planned-only apportionment base
    month_exec = Counter()       # planned Executive Missions per month (mapped counted below)
    for d, item in placed.items():
        if item[2] == "executive_mission":
            for (no, _n, days, first, *_r) in MONTHS:
                if first <= d < first + days:
                    month_exec[no] += 1
    for (no, _name, days, first, _theme, theme_doms) in MONTHS:
        cap = max(6, days // 3)
        for day in range(first, first + days):
            if day in placed:
                (c, t, ty, dom, life, sec, inter, diff, mins) = placed[day]
                entries[day] = {
                    "code": f"PI-Y1-D{day:03d}", "day": day, "status": "mapped", "source_challenge": c,
                    "title": t, "experience_type": ty, "primary_domain": dom,
                    "difficulty_band": DIFF_BAND[diff], "duration_band": duration_band(mins),
                    "est_minutes": mins, "interaction": inter, "lifecycle_stage": life, "sector": sec,
                }
                month_dom_count[(no, dom)] += 1
                continue
            dom_prefs = [d for d in theme_doms if month_dom_count[(no, d)] < cap] + \
                        sorted(DOMAINS, key=lambda d: -rd[d])
            dom = take(rd, dom_prefs, DOMAINS)
            month_dom_count[(no, dom)] += 1
            # Capstone missions are punctuation, not a diet: at most two planned Executive
            # Missions in any month (mapped ones are already spread by the placement above).
            exec_capped = month_exec[no] >= 2
            ty_prefs = [t for t in TYPE_FOR_DOMAIN[dom] if not (exec_capped and t == "executive_mission")]
            ty_pool = [t for t in TYPES if not (exec_capped and t == "executive_mission")] or TYPES
            ty = take(rt, ty_prefs, ty_pool)
            if ty == "executive_mission":
                month_exec[no] += 1
            inter = take(ri, INTER_FOR_TYPE[ty], INTERACTIONS)
            band = take(rb, BAND_FOR_TYPE[ty], BANDS)
            diff = take(rdi, DIFF_FOR_TYPE[ty], DIFFS)
            life = take(rl, LIFE_FOR_MONTH.get(no, []) + ["execution_control", "definition_planning"], LIFECYCLES)
            # Largest-remainder apportionment keeps every sector present all year round, instead
            # of the biggest quota monopolising the early months.
            sec = min((s2 for s2 in SECTORS if rs[s2] > 0),
                      key=lambda s2: ((sector_quota[s2] - rs[s2] + 1) / max(1, sector_quota[s2]), s2))
            rs[sec] -= 1
            mins = MINUTES[band][minute_idx[band] % len(MINUTES[band])]; minute_idx[band] += 1
            entries[day] = {
                "code": f"PI-Y1-D{day:03d}", "day": day, "status": "planned", "source_challenge": None,
                "title": title_for(dom, sec), "experience_type": ty, "primary_domain": dom,
                "difficulty_band": diff, "duration_band": band, "est_minutes": mins,
                "interaction": inter, "lifecycle_stage": life, "sector": sec,
            }

    # Within-month interleave: the day-by-day fill produces runs of one experience type (all the
    # theme-domain slots land first). Re-deal each month's PLANNED entries across its planned days
    # round-robin by type, largest group first — mapped days stay exactly where they were placed.
    for (no, _name, days, first, _theme, _doms) in MONTHS:
        planned_days = [d for d in range(first, first + days) if entries[d]["status"] == "planned"]
        groups = {}
        for d in planned_days:
            groups.setdefault(entries[d]["experience_type"], []).append(entries[d])
        order = sorted(groups, key=lambda t: (-len(groups[t]), t))
        dealt = []
        while any(groups[t] for t in order):
            for t in order:
                if groups[t]:
                    dealt.append(groups[t].pop(0))
        for d, e in zip(planned_days, dealt):
            e["day"] = d
            e["code"] = f"PI-Y1-D{d:03d}"
            entries[d] = e

    # every scheduled quota must be exactly exhausted
    for label, c in [("type", rt), ("domain", rd), ("difficulty", rdi), ("band", rb),
                     ("interaction", ri), ("lifecycle", rl), ("sector", rs)]:
        assert all(v == 0 for v in c.values()), f"unexhausted {label} quota: {c}"

    # reserve bank: 55 slots, type table only (the approved reserve constraint)
    reserve = []
    rt2 = Counter(RESERVE_TYPE)
    n = 0
    for ty in TYPES:
        for _ in range(RESERVE_TYPE[ty]):
            n += 1
            dom = TYPE_FOR_DOMAIN_INV.get(ty, DOMAINS)[n % len(TYPE_FOR_DOMAIN_INV.get(ty, DOMAINS))]
            sec = SECTORS[n % len(SECTORS)]
            band = BAND_FOR_TYPE[ty][0]
            diff = DIFF_FOR_TYPE[ty][0]
            mins = MINUTES[band][n % len(MINUTES[band])]
            reserve.append({
                "code": f"PI-Y1-R{n:03d}", "day": None, "status": "planned", "source_challenge": None,
                "title": title_for(dom, sec), "experience_type": ty, "primary_domain": dom,
                "difficulty_band": diff, "duration_band": band, "est_minutes": mins,
                "interaction": INTER_FOR_TYPE[ty][0], "lifecycle_stage": LIFECYCLES[n % len(LIFECYCLES)],
                "sector": sec,
            })
            rt2[ty] -= 1
    assert all(v == 0 for v in rt2.values()) and len(reserve) == 55

    # premium-language gate over every learner-facing string we are committing
    for e in list(entries.values()) + reserve:
        assert not BANNED.search(e["title"]), f"banned term in title: {e['title']}"

    os.makedirs(OUT, exist_ok=True)
    def dump(name, obj):
        with open(os.path.join(OUT, name), "w") as f:
            json.dump(obj, f, indent=1, ensure_ascii=False)
            f.write("\n")

    month_files = [f"{no:02d}-{name}.json" for (no, name, *_r) in MONTHS]
    dump("manifest.json", {
        "schema_version": 1,
        "product": "PCI Project Intelligence",
        "tagline": "Think. Decide. Deliver.",
        "generator": "backend/tools/gen_year1_intelligence.py",
        "scheduled_total": 365, "reserve_total": 55, "bank_total": 420,
        "month_files": month_files,
        "distributions": {
            "experience_type_scheduled": SCHED_TYPE, "experience_type_reserve": RESERVE_TYPE,
            "primary_domain": SCHED_DOMAIN, "difficulty_band": SCHED_DIFF,
            "duration_band": SCHED_BAND, "interaction": SCHED_INTER,
            "lifecycle_stage": SCHED_LIFE, "sector": SCHED_SECTOR,
            "monthly": {f"{no:02d}": days for (no, _n, days, *_r) in MONTHS},
        },
    })
    for (no, name, days, first, theme, _doms) in MONTHS:
        dump(f"{no:02d}-{name}.json", {
            "schema_version": 1, "month": no, "name": name, "theme": theme,
            "entries": [entries[d] for d in range(first, first + days)],
        })
    dump("reserve.json", {"schema_version": 1, "entries": reserve})

    # human index
    lines = ["# PCI Project Intelligence — Year-1 content index", "",
             "Generated by `backend/tools/gen_year1_intelligence.py` — do not edit the JSON by hand;",
             "change the generator (or the C# classification) and regenerate. CI verifies every",
             "distribution table, code range and mapped-slot assignment.", "",
             "| Month | Theme | Days | Codes | Mapped | Planned |", "|---|---|---:|---|---:|---:|"]
    for (no, name, days, first, theme, _doms) in MONTHS:
        month_entries = [entries[d] for d in range(first, first + days)]
        mapped_n = sum(1 for e in month_entries if e["status"] == "mapped")
        lines.append(f"| {no:02d} {name.capitalize()} | {theme} | {days} | PI-Y1-D{first:03d}–D{first+days-1:03d} "
                     f"| {mapped_n} | {days - mapped_n} |")
    total_mapped = sum(1 for e in entries.values() if e["status"] == "mapped")
    lines += ["", f"Scheduled: **365** ({total_mapped} mapped to the published bank, {365 - total_mapped} planned).",
              "Reserve: **55** (PI-Y1-R001–R055, all planned).",
              "", "Mapped slots carry `source_challenge` (the immutable bank code). Audit status for every",
              "mapped item: *conditionally accepted — remediation required* (progressive hints, coach",
              "boundaries, review-by dates). See `docs/pciworld/PROJECT_INTELLIGENCE.md`."]
    with open(os.path.join(OUT, "YEAR1_CONTENT_INDEX.md"), "w") as f:
        f.write("\n".join(lines) + "\n")

    print(f"wrote {len(month_files) + 3} files to {os.path.relpath(OUT)}"
          f" — scheduled 365 ({total_mapped} mapped), reserve 55")


# domains each type most naturally practises, for reserve slots (inverse of TYPE_FOR_DOMAIN)
TYPE_FOR_DOMAIN_INV = {
    "daily_decision": ["integration_governance", "change_claims_recovery", "procurement_contracts",
                       "quality_assurance", "digital_data_ai", "safety_sustainability"],
    "project_rescue": ["change_claims_recovery", "schedule_planning", "cost_commercial", "risk_uncertainty"],
    "risk_room": ["risk_uncertainty"],
    "schedule_strategy": ["schedule_planning", "resources_leadership"],
    "cost_value": ["cost_commercial", "procurement_contracts"],
    "stakeholder_dilemma": ["stakeholders_communication", "resources_leadership", "integration_governance"],
    "logic_sequence": ["scope_requirements", "schedule_planning", "quality_assurance"],
    "executive_mission": ["integration_governance"],
}

if __name__ == "__main__":
    main()
