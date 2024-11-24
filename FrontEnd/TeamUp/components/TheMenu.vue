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

<!-- <template>
  <div>
  <Menubar class="justify-center gap-8 flex hover:scale-[101%] transition cursor-pointer hero" >
    <MenubarMenu v-for="(item, index) in items" :key="index" >
      <a :href="item.path" className="text-slate-400">
        <MenubarTrigger class="flex items-center gap-2 px-2 py-1 transition 
        rounded cursor-pointer hover:bg-neutral-100 text-lg">{{ item.label }}</MenubarTrigger>
      </a>
    </MenubarMenu>
  </Menubar>
</div>
</template> -->

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
    <AddPlayer></AddPlayer>
  </div>
</template>



<style scoped>
.hero {
  background-color: #e8e8d9;
}
</style>
