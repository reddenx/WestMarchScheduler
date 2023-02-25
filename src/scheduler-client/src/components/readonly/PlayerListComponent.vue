<template>
  <div class="container">
    <div class="row">
      <div
        class="col-12 col-lg-1"
        v-for="player in session.players"
        :key="player.name"
      >
        {{ player.name }}
      </div>
    </div>
  </div>
</template>

<script>
import { SessionViewmodel } from "../../scripts/CommonModels";
export default {
  props: {
    session: SessionViewmodel,
  },
};
</script>

<style>
</style>