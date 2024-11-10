<template>
  <UForm
    :validate="validateForm"
    :state="state"
    class="space-y-4"
    @submit="onSubmit"
    @error="onError"
  >
    <UFormGroup label="Team name" name="Name">
      <UInput v-model="state.Name" type="text" />
    </UFormGroup>

    <UFormGroup label="Members" name="Members">
      <USelectMenu v-model="state.Members" :options="playerOptions" multiple placeholder="Select players" />
    </UFormGroup>

    <UButton type="submit">Add Team</UButton>
  </UForm>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive } from "vue";
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Team } from "~/types/team";
import { useTeamStore } from "~/stores/teamStore";
import { usePlayerStore } from "~/stores/playerStore";

const playerStore = usePlayerStore();
const teamStore = useTeamStore();

onMounted(() => {
  playerStore.loadPlayers();
});

const playerOptions = computed(() =>
  playerStore.players.map((player) => ({
    value: player,
    label: player.name,
  }))
);

const state = reactive<Team>({
  Id: teamStore.generateId(),
  Name: '',
  Members: [],
});

const validateForm = (state: Team): FormError[] => {
  const errors: FormError[] = [];

  if (!state.Name) {
    errors.push({ path: "Name", message: "Team name is required" });
  }

  if (state.Members.length < 2) {
    errors.push({ path: "Members", message: "Choose at least 2 players" });
  }

  const unassignedPlayers = state.Members.every(member => member.value.teamId === null);
  if (!unassignedPlayers) {
    errors.push({ path: "Members", message: "One or more selected players are already in a team" });
  }

  return errors;
};

const transformTeamData = () => ({
  Id: state.Id,
  Name: state.Name,
  Members: state.Members.map(({ value: { id, name, points, rank, teamId } }) => ({
    id,
    name,
    points,
    rank,
    teamId,
  })),
});

async function onSubmit(event: FormSubmitEvent) {
  try {
    const teamData = transformTeamData();
    await teamStore.addTeam(teamData);
    console.log("Team successfully added:", teamData);
    await navigateTo("/teams");
  } catch (error) {
    console.error("Error in addTeam:", error);
  }
}

async function onError(event: FormErrorEvent) {
  const firstErrorElement = document.getElementById(event.errors[0].id);
  firstErrorElement?.focus();
  firstErrorElement?.scrollIntoView({ behavior: "smooth", block: "center" });
}
</script>
