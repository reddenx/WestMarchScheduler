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
        <div v-if="session.status == 'Posted'">
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
        <div v-if="session.status == 'Approved'">
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
        <div v-if="session.status == 'Open'">
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
        <div v-if="session.status == 'Finalized'">
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
  props: {
    session: SessionViewmodel,
  },
  data: () => ({
  }),
  mounted() {},
  methods: {},
};
</script>

<style>
</style>