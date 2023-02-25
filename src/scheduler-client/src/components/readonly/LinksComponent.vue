<template>
  <div class="container">
    <div class="row">
        <div class="col-12 col-lg-4">
            Host: {{session.host.name}} 
        </div>
        <div class="col-12 col-lg-4">
            Lead: {{session.lead.name}}
        </div>
        <div class="col-12 col-lg-4">
            Player Link
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

}
</script>

<style>
</style>