// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2024-04-03',
  devtools: { enabled: true },
  modules: ['@nuxt/ui', '@pinia/nuxt', '@nuxt/fonts'],
  imports :{dirs: ["types/*.ts"]},
  runtimeConfig: {
    //siin on https või ilma s (enne oli s'ga)
    public: {exercisesApiUrl: "https://localhost:5181/api/"}
  },
});