<template>
    <div class="container mt-2">
        <div class="card" v-if="session.players.length">
            <div class="card-header">Players</div>
            <ul class="list-group list-group-flush">
                <li
                    v-for="player in session.players"
                    :key="player.name"
                    class="list-group-item"
                >
                    {{ player.name }}
                </li>
            </ul>
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