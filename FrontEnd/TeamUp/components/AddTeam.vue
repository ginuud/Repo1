<template>
  <UButton
    :ui="{ rounded: 'rounded-full' }"
    icon="i-heroicons-plus"
    size="md"
    variant="solid"
    label="Add team"
    @click="openStartTeamModal"
    class="m-4"
    :style="{ backgroundColor: '#202a79', color: '#fff' }"
  >
  </UButton>
  <UModal v-model="isStartTeamModalOpen" prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-900',
      }"
      >
      <template #header>          
        <div class="flex items-center justify-between">
          <h3 class="text-base font-semibold leading-6 text-white "
            >Add team
          </h3>        
        <UButton
          color="gray"
          variant="ghost"
          icon="i-heroicons-x-mark-20-solid"
          class="-my-1"
          @click="isStartTeamModalOpen = false"
        />
        </div>
      </template>

      <UTabs :items="tabs" class="w-full">
        <template #item="{ item }">
          <div>
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
                    color="purple"
                    variant="outline"
                  />
                </UFormGroup>
                <p class="text-gray-500">Only players who are not already in a team are shown</p>

                <UFormGroup label="Number of Teams" name="numberOfTeams">
                  <UInput v-model.number="generateTeamsForm.numberOfTeams" type="number" color="purple"
                  variant="outline"/>
                </UFormGroup>

                <UFormGroup label="Team Names" name="teamNames" >
                  <div v-for="index in generateTeamsForm.numberOfTeams" :key="index" style="margin-bottom: 0.5rem;">
                    <UInput v-model="generateTeamsForm.teamNames[index - 1]" type="text" placeholder="Enter team name" color="purple"
                    variant="outline"/>
                  </div>
                </UFormGroup>

                <UButton type="submit">Generate teams</UButton>
                <UButton 
                  type="button"
                  @click="cancel" 
                  class="ml-2"
                  color="red">
                  Cancel
                </UButton>
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
                  <UInput v-model="addTeamForm.name" color="purple" variant="outline"/>
                </UFormGroup>

                <UFormGroup label="Members" name="members">

                  <USelectMenu 
                    v-model="addTeamForm.members" 
                    :options="playerOptions" 
                    multiple 
                    searchable
                    searchable-placeholder="Search a player..."
                    placeholder="Select players"
                    color="purple"
                    variant="outline"
                  />
                </UFormGroup>
                <div style="margin-bottom: 0.5rem;">
                  <p class="text-gray-500">Only players who are not already in a team are shown</p>
                </div>

                <UButton type="submit">Add team</UButton>
                <UButton 
                  type="button"
                  @click="cancel" 
                  class="ml-2"
                  color="red">
                  Cancel
                </UButton>
              </UForm>
            </div>
          </div>
        </template>
      </UTabs>
    </UCard>
  </UModal>
</template>
  


<script setup lang="ts">
import { computed, reactive, onMounted, watch } from "vue";
import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import { useTeamStore } from "~/stores/teamStore";
import { usePlayerStore } from "~/stores/playerStore";
import { useRouter } from 'vue-router';

const teamStore = useTeamStore();
const playerStore = usePlayerStore();
const router = useRouter(); 
const isStartTeamModalOpen = ref(false);

onMounted(async () => {
  await playerStore.loadPlayers();
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

function resetAddTeamForm() {
  addTeamForm.id = teamStore.generateId();
  addTeamForm.name = "";
  addTeamForm.members = [];
}

const openStartTeamModal = async () => {
  await playerStore.loadPlayers()
  isStartTeamModalOpen.value = true;	  
};

watch(playerOptions, (newOptions) => {
  generateTeamsForm.selectedPlayers = [...newOptions];
}, { immediate: true });


const validateGenerateTeams = (state: typeof generateTeamsForm): FormError[] => {
  const errors: FormError[] = [];
  if (state.selectedPlayers.length < 3) {
    errors.push({ path: "selectedPlayers", message: "Choose at least 3 players" });
  }
  if (state.numberOfTeams < 2 || state.numberOfTeams > 20) {
    errors.push({ path: "numberOfTeams", message: "At least 2 teams required and not over 20" });
  }
  return errors;
};

const validateAddTeam = (state: typeof addTeamForm): FormError[] => {
  const errors: FormError[] = [];
  console.log("addTeamForm", addTeamForm)
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
    await router.push("/teams");
    isStartTeamModalOpen.value = false;
  } catch (error) {
    console.error("Error generating teams:", error);
  }
  await teamStore.loadTeams()
}

async function submitAddTeam(event: FormSubmitEvent) {
  event.preventDefault();
  console.log("addTeamForm", addTeamForm)
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
    console.log("teamdata", teamData)
    await teamStore.addTeam(teamData);
    console.log("Team successfully added:", teamData);
    await router.push("/teams");
    isStartTeamModalOpen.value = false;
    resetAddTeamForm()
  } catch (error) {
    console.error("Error adding team:", error);
  }
}

function handleError(event: FormErrorEvent) {
  const firstErrorElement = document.getElementById(event.errors[0]?.id);
  firstErrorElement?.focus();
  firstErrorElement?.scrollIntoView({ behavior: "smooth", block: "center" });
}

const cancel = () => {
  isStartTeamModalOpen.value = false;
};

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

