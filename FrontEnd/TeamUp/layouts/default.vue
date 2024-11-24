<template>
  <div>
    <div class="flex items-center justify-between px-8 py-2">
      <TheMenu />
      <div class="flex-1"></div>
      <div><AddPlayer class="items-center"/></div>
    </div>
    <div class="mt-4">
      <slot />
    </div>
  </div>
</template>
