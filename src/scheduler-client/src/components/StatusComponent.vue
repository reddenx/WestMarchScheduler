<template>
  <div class="card">
    <div class="card-body">
      <div class="row">
        <div class="col-12">
          <!-- TODO: add status components by lookuptype, then status -->
          <div class="badge bg-info text-dark float-end">
            {{ session.status }}
          </div>
          <h1>
            {{ session.title }}
          </h1>
          <hr />
          <p class="text-start">
            {{ session.description }}
          </p>
          <hr />

          <div v-if="session.lookupType === 'lead'">
            <div v-if="session.status === 'Posted'">
              Pass this link off to your host so they can set their schedule
              <router-link class="alert-link" :to="`/${session.hostKey}`">{{
                session.hostKey
              }}</router-link>
            </div>
            <SetLeadScheduleComponent :session="session" v-if="session.status === 'Approved'"
              @submitted="handleLeadScheduled" />
            <FinalizeComponent v-if="session.status === 'Open'" :session="session" />
            <hr />
          </div>

          <div v-if="session.lookupType === 'host'">
            <SetHostScheduleComponent :hostKey="session.hostKey" v-if="session.status === 'Posted'"
              @submitted="handleHostApproved" />
            <div class="" v-if="session.status === 'Approved'">
              Let your organizer know that you've approved the event!
            </div>
            <FinalizeComponent v-if="session.status === 'Open'" :session="session" />
            <hr />
          </div>

          <div v-if="session.lookupType === 'player'">
            <div v-if="session.status === 'Posted'">
              This event is waiting for the organizers to setup an initial schedule
            </div>
            <div v-if="session.status === 'Approved'">
              This event is waiting for the organizers to setup an initial schedule
            </div>
            <PlayerJoinComponent :session="session" @submitted="handlePlayerSubmitted" v-if="session.status === 'Open'" />
            <hr />
          </div>

          <div class="text-start">
            <div class="alert alert-danger" v-show="session.leadKey">
              Lead Link:
              <router-link class="alert-link" :to="`/${session.leadKey}`">{{
                session.leadKey
              }}</router-link>
            </div>
            <div class="alert alert-warning" v-show="session.hostKey">
              Host Link:
              <router-link class="alert-link" :to="`/${session.hostKey}`">{{
                session.hostKey
              }}</router-link>
            </div>
            <div class="alert alert-success">
              Player Link:
              <router-link class="alert-link" :to="`/${session.playerKey}`">{{
                session.playerKey
              }}</router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { SessionViewmodel } from "../scripts/CommonModels";
import SetHostScheduleComponent from "../components/SetHostScheduleComponent.vue";
import SetLeadScheduleComponent from "../components/SetLeadScheduleComponent.vue";
import PlayerJoinComponent from "../components/PlayerJoinComponent";
import FinalizeComponent from "../components/FinalizeComponent";

export default {
  components: {
    SetHostScheduleComponent,
    SetLeadScheduleComponent,
    PlayerJoinComponent,
    FinalizeComponent,
  },
  props: {
    session: SessionViewmodel,
  },
  methods: {
    handleHostApproved() {
      this.$emit("reload");
    },
    handleLeadScheduled(){
      this.$emit("reload");
    },
    handlePlayerSubmitted() {
      this.$emit("reload");
    }
  },
};
</script>

<style>

</style>