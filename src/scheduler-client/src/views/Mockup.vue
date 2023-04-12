<template>
  <div class="container">
    <div class="row">
      <div class="col-12">
        <!-- DONE -->
        <StatusComponent :session="session" />
      </div>
      <div class="col-12 mb-3">
        <!-- done enough for now -->
        <InfoComponent :session="session" />
      </div>
      <div class="col-12 col-lg-6 order-lg-2">
        <div v-if="session.status == 'posted'">
          <div v-if="session.lookupType == 'lead'">
            TODO TOP CALLOUT: Send this to your host, they'll add their schedule and you can move on
          </div>
          <div v-if="session.lookupType == 'host'">
            CTA: Add your availability so the lead can pick a schedule
          </div>
          <div v-if="session.lookupType == 'player'">
            TOP CALLOUT?: Sit tight, this is still in negotiation between your leader and host
          </div>
        </div>
        <div v-if="session.status == 'approved'">
          <div v-if="session.lookupType == 'lead'">
            CTA: Choose all your available times
          </div>
          <div v-if="session.lookupType == 'host'">
            TOP CALLOUT: Send this to your lead, they will narrow down their available times
          </div>
          <div v-if="session.lookupType == 'player'">
            TOP CALLOUT? Sit tight, this is still in negotiation between your leader and host
          </div>
        </div>
        <div v-if="session.status == 'open'">
          <div v-if="session.lookupType == 'lead'">
            CTA: You can close and choose the final schedule
          </div>
          <div v-if="session.lookupType == 'host'">
            TOP CALLOUT? Sit tight, the lead will choose the final schedule when enough players have joined
          </div>
          <div v-if="session.lookupType == 'player'">
            CTA: Join Now!
          </div>
        </div>
        <div v-if="session.status == 'finalized'">
          Everything should be frozen
        </div>
      </div>
      <div class="col-12">
        <LinksComponent :session="session" />
      </div>
      <div class="col-12 col-lg-6 order-lg-1">
        <PlayerListComponent :session="session" />
      </div>
    </div>
  </div>
</template>

<script>
import InfoComponent from "../components/readonly/InfoComponent.vue";
import StatusComponent from "../components/readonly/StatusComponent.vue";
import PlayerListComponent from "../components/readonly/PlayerListComponent.vue";
import LinksComponent from "../components/readonly/LinksComponent.vue";
import { SessionViewmodel } from "../scripts/CommonModels";
import { SessionDto } from "../scripts/SessionApi";

export default {
  components: {
    InfoComponent,
    StatusComponent,
    LinksComponent,
    PlayerListComponent,
  },
  data: () => ({
    session: new SessionViewmodel(
      new SessionDto(
        "playerkey",
        "hostKey",
        "leadKey",
        "posted",
        "let's go get that druid compound",
        "A long description here, sometimes people tend to write a very long description. maybe let's go to the next town over and murder a bunch of people assuming they're undead before realizing there was just an outbreak of cholera. and now have to disguise yourselves on the main road anywhere near that region",
        {
          name: "SeanHost",
          schedule: [
            {
              start: new Date(2023, 2, 24, 13).toISOString(),
              end: new Date(2023, 2, 24, 17).toISOString(),
            },
          ],
        },
        {
          name: "DraunoLead",
          schedule: [
            {
              start: new Date(2023, 2, 24, 13).toISOString(),
              end: new Date(2023, 2, 24, 17).toISOString(),
            },
          ],
        },
        [
          {
            name: "Jonk",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
          {
            name: "Jimbo",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
          {
            name: "Himby",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
          {
            name: "Halls",
            schedule: [
              {
                start: new Date(2023, 2, 24, 13).toISOString(),
                end: new Date(2023, 2, 24, 17).toISOString(),
              },
            ],
          },
        ],
        null,
        "hour"
      ),
      "leadKey"
    ),
  }),
  mounted() {},
  methods: {},
};
</script>

<style>
</style>