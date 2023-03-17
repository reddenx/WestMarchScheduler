<template>
    <div class="container">
        <div class="row mt-2">
            <div
                class="col-3 status-container"
                v-for="(status, index) in statuses"
                :key="status"
                :class="{
                    current: index == statusIndex,
                    incomplete: index > statusIndex,
                    completed: index < statusIndex,
                }"
            >
                <div v-show="index > 0" class="bar-left"></div>
                <div v-show="index < 3" class="bar-right"></div>

                <div class="orb-container">
                    <div class="orb">
                        {{ index + 1 }}
                    </div>
                </div>

                <div>
                    {{ status }}
                </div>
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
    data: () => ({
        statuses: ["posted", "approved", "open", "finalized"],
        statusIndex: 0,
    }),
    mounted() {
        this.statusIndex = this.statuses.indexOf(this.session.status);
    },
};
</script>

<style>
.current {
    color: #3498db;
}
.current .orb {
    background-color: #3498db;
    color: #fff;
}
.current .bar-right {
    background-color: #dfe3e4;
}
.current .bar-left {
    background-color: #2ecc71;
}

.incomplete {
    color: #b4b4b4;
}
.incomplete .orb {
    background-color: #dfe3e4;
    color: #b4b4b4;
}
.incomplete .bar-right {
    background-color: #dfe3e4;
}
.incomplete .bar-left {
    background-color: #dfe3e4;
}

.completed {
    color: #2ecc71;
}
.completed .orb {
    background-color: #2ecc71;
    color: #fff;
}
.completed .bar-right {
    background-color: #2ecc71;
}
.completed .bar-left {
    background-color: #2ecc71;
}

.bar-right {
    width: 50%;
    position: absolute;
    right: 0;
    top: 0.75em;
    height: 1em;
}
.bar-left {
    width: 50%;
    position: absolute;
    left: 0;
    top: 0.75em;
    height: 1em;
}

.orb {
    margin: auto;
    width: 2.5em;
    height: 2.5em;
    border-radius: 2.5em;
    display: flex;
    align-items: center;
    justify-content: center;
}
.orb-container {
    position: relative;
}
.status-container {
    position: relative;
    text-transform: capitalize;
    padding: 0;
}
</style>