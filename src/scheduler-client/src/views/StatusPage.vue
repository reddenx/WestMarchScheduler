<template>
    <div>Status: {{ lookupKey }}</div>
</template>

<script>
import Api from "../scripts/SessionApi";
import {SessionViewmodel, PlayerViewmodel, ScheduleViewmodel} from "../scripts/CommonModels";

const api = new Api();

export default {
    props: {
        lookupKey: String,
    },
    async mounted() {
        let dto = await api.getSession(this.lookupKey);
        console.log(dto);
    },
};
</script>

<style>
</style>