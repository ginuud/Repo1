<template>
  <UButton
    :ui="{rounded: 'rounded-full'}"
    icon="i-heroicons-plus"
    size="md"
    variant="solid"
    label="Add player"
    :trailing="false"
    @click="openAddPlayerModal"
    class="m-4"
    :style="{ backgroundColor: '#202a79', color: '#fff' }"
  >
  </UButton>

  <UModal v-model="isAddPlayerModalOpen" prevent-close>
    <UCard
      :ui="{
        ring: '',
        divide: 'divide-y divide-gray-100',
      }"
    >
      <template #header>
        <div class="flex items-center justify-between">
          <UFormGroup label="Add player" name="addPlayer" ></UFormGroup>
          <UButton
            color="gray"
            variant="ghost"
            icon="i-heroicons-x-mark-20-solid"
            class="-my-1"
            @click="isAddPlayerModalOpen = false"
          />
        </div>
      </template>

      <div class="p-4">
        <UFormGroup label="Player name" name="name">
          <UInput
            v-model="name"
            color="purple"
            variant="outline"
            placeholder="Player name"
          />
          <p v-if="errors.name" class="text-red-500 text-sm mt-1">
            {{ errors.name }}
          </p>
        </UFormGroup>
        <UFormGroup label="Points" name="points">
          <UInput
            v-model="points"
            type="number"
            color="purple"
            variant="outline"
            placeholder="Player points"
          />
        </UFormGroup>
      </div>

      <template #footer>
        <div class="flex justify-end space-x-2">
          <UButton color="green" @click="addPlayer">Add player</UButton>
        </div>
      </template>
    </UCard>
  </UModal>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { usePlayerStore } from "~/stores/playerStore";

const playerStore = usePlayerStore();

const isAddPlayerModalOpen = ref(false);
const name = ref("");
const points = ref<number>(0);
const errors = reactive<{ name: string | null }>({
  name: null,
});

const validate = () => {
  errors.name = name.value ? null : "Required";
  return !errors.name;
};

const openAddPlayerModal = () => {
  isAddPlayerModalOpen.value = true;
  points.value = 0;
  name.value = "";
};

const addPlayer = async () => {
  if (validate()) {
    await playerStore.addPlayer({
      id: playerStore.generateId(),
      name: name.value,
      points: points.value,
      rank: 0,
    });
    isAddPlayerModalOpen.value = false;
    name.value = "";
    points.value = null;
  }
};
</script>

