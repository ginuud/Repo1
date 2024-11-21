<template>
  <UForm
    :validate="validateForm"
    :state="state"
    class="space-y-4"
    @submit="onSubmit"
    @error="onError"
  >
    <UFormGroup label="Team name" name="name">
      <UInput v-model="state.name" type="name"/>
    </UFormGroup>

    <UFormGroup label="Members" name="members">
      <USelectMenu 
        v-model="state.members" 
        :options="playerOptions" 
        multiple 
        searchable
        searchable-placeholder="Search a player..."
        placeholder="Select players" 
      />
    </UFormGroup>
    <p class="text-gray-500">Only players who are not already in a team are shown</p>

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
  playerStore.players
    .filter((player) => player.teamId === null)
    .map((player) => ({
      value: player,
      label: player.name,
    }))
);

const state = reactive<Team>({
  id: teamStore.generateId(),
  name: '',
  members: [],  
});

watch(playerOptions, (newOptions) => {
        state.members = [...newOptions]; // Initialize selectedPlayers with all players
      }, { immediate: true });

const validateForm = (state: Team): FormError[] => {
  const errors: FormError[] = [];

  if (!state.name) {
    errors.push({ path: "Name", message: "Team name is required" });
  }

  if (state.members.length < 2) {
    errors.push({ path: "Members", message: "Choose at least 2 players" });
  }

  return errors;
};

const transformTeamData = () => ({
  Id: state.id,
  Name: state.name,
  Members: state.members.map(({ value: { id, name, points, rank, teamId } }) => ({
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
