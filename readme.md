# Project Goals
- simple D&D group scheduling system

# Features
- simple sharable links
- no accounts necessary
- simple UI

# Process
- LEAD player kicks off the process with a quest idea
- DM selects approves topic and adds their schedule options
- LEAD selects all options that they can attend, this opens the quest up for general joining
- other players join adding their availabilities
- DM closes joining and selects a date

## Workflow Details
- As a person organizing(lead) an event, I'd like to /create an event and afterwards be given links to check in:
  + A link to follow up on it's progress (lead link)
  + A link to give to my host to approve (host link)
  + A link to give to players that would like to join (player join link)
- As a person hosting(host) an event, I'd like to receive a link from the lead that takes me to an approval page, after entering my schedule and approving the event, I'd like to be given links:
  + A link to follow up on it's progress (host link)
  + A link to distribute to players (player join link)
- As a person joining (player) an event, I'd like to receive a link from either the lead or host that lets me join, afterwards I will be given a link
  + A link to follow up on it's progress (player status link)
  + the player join link (using local storage) will also show the player status page
- After enough players or the lead or host decides, the lead or host will finalize the schedule, afterwards they will be presented with a public status link to share (player status link)

## Pages determined from workflow
- (/) Homepage directing users to the create page
- (/create) Lead Create Page
+ (/{leadkey}) Lead status page
  - if **Posted**, show all links, recommend bugging host to approve
  - if **Approved**, show schedule component overlayed with host schedule which submits the lead schedule
  - if **open**, show link to give to players, finalize component with overlaying player and host schedules which submits the final schedule
  - if **finalized**, show finalized status component
+ (/{playerkey}) player status page
  - if **posted**, show very general status
  - if **approved**, show very general status
  - if **open**, show player join component with overlapping host & lead schedules
  - if **finalized**, show finalized status component
+ (/{hostkey}) host status page
  - if **posted**, show approve component
  - if **approved**, show very general status
  - if **open**, show very general status, finalize component with overlaying player and host schedules which submits the final schedule
  - if **finalized**, show finalized status component