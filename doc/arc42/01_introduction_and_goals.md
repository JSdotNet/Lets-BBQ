[back to index](../ARC42.md)

# Introduction and Goals

## 1.1 Requirements Overview

**System Purpose:**
Lets BBQ (working title) helps friend groups organize BBQ events by centralizing scheduling, logistics, and contribution tracking in a single collaborative space.

### Essential Features

- Schedule BBQ events by proposing and finalizing date/time slots.
- Capture and share the event location plus practical notes (parking, indoor backup, etc.).
- Assign responsibilities so organizers and guests know who brings each item.
- Track shared costs and equalize expenses across attendees.
- Offer cut-specific cooking guides and recipe suggestions tailored to the planned menu.

### Business Context

The application targets casual organizers who repeatedly juggle messaging threads, spreadsheets, and payment apps to coordinate BBQs. Consolidating decisions (when, where, what to bring, and how to split costs) reduces planning overhead, improves guest engagement, and ensures every gathering runs smoothly with balanced contributions.

### References

- Requirements Document: None yet (TBD) – no formal requirements specification is available.

---

## 1.2 Quality Goals

| Priority | Quality Goal | Concrete Scenario |
|:--------:|-------------|-------------------|
| **1** | #reliable — Always-accessible event info | Maintain 99.5% monthly uptime for organizer and guest actions (scheduling, RSVPs, contribution updates), excluding planned maintenance windows announced 24 hours in advance. |
| **2** | #efficient — Snappy interactions | Keep p95 response times under 250 ms for core API calls (load event dashboard, submit RSVP, update contribution) with up to 500 concurrent users, verified via load tests before each release. |
| **3** | #efficient/#operable — Cost-aware hosting | Limit average hosting expenses to under EUR 40 per month for up to 1,000 monthly active users by leveraging free tiers and suspending idle background jobs within 15 minutes of inactivity. |

⚠️ These goals remain pending formal approval from organizer and guest representatives and must be referenced by the detailed scenarios planned for Section 10.

---

## 1.3 Stakeholders

| Role/Name | Contact | Expectations from Architecture/Documentation |
|-----------|---------|----------------------------------------------|
| Organizer (Event Host) | N/A (role-based) | Needs clarity on how the system supports planning, communication, and cost tracking so events stay coordinated. |
| Invited Guest | N/A (role-based) | Expects transparent access to event details, contributions, and preparation guidance with minimal friction. |
| Developer / Architecture Owner (JSdotNet) | N/A (internal) | Requires stable architectural decisions, documented quality goals, and traceability for future enhancements. |

### Stakeholder Categories

- **End Users:** Organizers and invited guests rely on the app for accurate schedules, locations, and contribution visibility.
- **Development Team:** Sole developer (JSdotNet) maintains architecture consistency, captures decisions, and ensures future contributors can onboard quickly.