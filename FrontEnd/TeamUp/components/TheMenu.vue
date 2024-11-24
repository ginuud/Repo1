<script setup lang="ts">
const route = useRoute();

const links = ref([
  {
    label: "Players",
    to: "/players",
  },
  {
    label: "Teams",
    to: "/teams",
  },
  {
    label: "Games",
    to: "/games",
  },
]);
</script>

<template>
  <div class="flex items-center justify-between px-8 py-2">
    <!-- Logo Image -->
    <img src="@/components/teamup.png" alt="TeamUp logo" class="w-24 h-auto" />

    <!-- Spacer -->
    <div class="flex-1 pl-10"></div>

    <!-- Navigation Bar -->
    <UHorizontalNavigation
      v-if="route.name !== 'index'"
      :links="links"
      class="hero"
      :ui="{
        after: '',
        active: 'dark:text-[#202a79]',
        base: 'rounded-md font-medium text-xl px-4 py-8',
      }"
    />
    <AddPlayer/>
  </div>
</template>

<style scoped>
.hero {
  background-color: #e8e8d9;
}
</style>
