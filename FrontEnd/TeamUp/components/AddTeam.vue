<template>
  <UTabs :items="tabs" class="w-full">
    <template #item="{ item }">
      <UCard>
        <template #header>
          <p class="text-base font-semibold leading-6 text-gray-900 dark:text-white">
            {{ item.label }}
          </p>
          <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">
            {{ item.description }}
          </p>
        </template>

        <div v-if="item.key === 'generateTeams'" class="space-y-4">
          <UForm
            :validate="validateGenerateTeams"
            :state="generateTeamsForm"
            @submit="submitGenerateTeams"
            @error="handleError"
          >
            <UFormGroup label="Select Players" name="selectedPlayers">
              <USelectMenu 
                v-model="generateTeamsForm.selectedPlayers" 
                :options="playerOptions" 
                multiple 
                searchable
                searchable-placeholder="Search a player..."
                placeholder="Select players"
              >
              </USelectMenu>
            </UFormGroup>

            <p class="text-gray-500">Only players who are not already in a team are shown</p>

            <UFormGroup label="Number of Teams" name="numberOfTeams">
              <UInput v-model.number="generateTeamsForm.numberOfTeams" type="number" />
            </UFormGroup>

            <UFormGroup label="Team Names" name="teamNames">
              <div v-for="index in generateTeamsForm.numberOfTeams" :key="index">
                <UInput v-model="generateTeamsForm.teamNames[index - 1]" type="text" placeholder="Enter team name" />
              </div>
            </UFormGroup>

            <UButton type="submit">Generate Teams</UButton>
          </UForm>
        </div>

        <div v-else-if="item.key === 'addTeam'" class="space-y-4">
          <UForm
            :validate="validateAddTeam"
            :state="addTeamForm"
            @submit="submitAddTeam"
            @error="handleError"
          >
            <UFormGroup label="Team Name" name="name">
              <UInput v-model="addTeamForm.name" />
            </UFormGroup>

            <UFormGroup label="Members" name="members">
              <USelectMenu 
                v-model="addTeamForm.members" 
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
        </div>
      </UCard>
    </template>
  </UTabs>
</template>


<script setup lang="ts">
import { computed, reactive, onMounted, watch } from "vue";
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import { useTeamStore } from "~/stores/teamStore";
import { usePlayerStore } from "~/stores/playerStore";

const teamStore = useTeamStore();
const playerStore = usePlayerStore();

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

const generateTeamsForm = reactive({
  selectedPlayers: [],
  numberOfTeams: 2,
  teamNames: [],
});

const addTeamForm = reactive({
  id: teamStore.generateId(),
  name: "",
  members: [],
});


watch(playerOptions, (newOptions) => {
  generateTeamsForm.selectedPlayers = [...newOptions];
}, { immediate: true });


const validateGenerateTeams = (state: typeof generateTeamsForm): FormError[] => {
  const errors: FormError[] = [];
  if (state.selectedPlayers.length < 4) {
    errors.push({ path: "selectedPlayers", message: "Choose at least 4 players" });
  }
  if (state.numberOfTeams < 2) {
    errors.push({ path: "numberOfTeams", message: "At least 2 teams required" });
  }
  return errors;
};

const validateAddTeam = (state: typeof addTeamForm): FormError[] => {
  const errors: FormError[] = [];
  if (!state.name) {
    errors.push({ path: "name", message: "Team name is required" });
  }
  if (state.members.length < 2) {
    errors.push({ path: "members", message: "Choose at least 2 players" });
  }
  return errors;
};

async function submitGenerateTeams(event: FormSubmitEvent) {
  event.preventDefault();
  try {
    const transformedPlayers = generateTeamsForm.selectedPlayers.map(({ value }) => ({
      id: value.id,
      name: value.name,
      points: value.points,
      rank: value.rank,
      teamId: value.teamId,
    }));
    await teamStore.generateTeams(transformedPlayers, generateTeamsForm.numberOfTeams, generateTeamsForm.teamNames);
    console.log("Teams successfully generated");
    await navigateTo("/teams");
  } catch (error) {
    console.error("Error generating teams:", error);
  }
}

async function submitAddTeam(event: FormSubmitEvent) {
  event.preventDefault();
  try {
    const teamData = {
      id: addTeamForm.id,
      name: addTeamForm.name,
      members: addTeamForm.members.map(({ value }) => ({
        id: value.id,
        name: value.name,
        points: value.points,
        rank: value.rank,
        teamId: value.teamId,
      })),
    };
    await teamStore.addTeam(teamData);
    console.log("Team successfully added:", teamData);
    await navigateTo("/teams");
  } catch (error) {
    console.error("Error adding team:", error);
  }
}

function handleError(event: FormErrorEvent) {
  const firstErrorElement = document.getElementById(event.errors[0]?.id);
  firstErrorElement?.focus();
  firstErrorElement?.scrollIntoView({ behavior: "smooth", block: "center" });
}

const tabs = [
  {
    key: "generateTeams",
    label: "Generate Teams",
    description: "Unselect players who you DO NOT want in the team and specify the number of teams to generate.",
  },
  {
    key: "addTeam",
    label: "Add Team",
    description: "Create a custom team by selecting players and naming it.",
  },
];
</script>

