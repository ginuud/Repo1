import type { User } from "~/types/user";
import type { NitroFetchOptions, NitroFetchRequest } from "nitropack";
import { useApi } from "./useApi";

export const useAuth = () => {
  const activeToken = useState<string | undefined>("token", () => undefined);

  const api = useApi();

  const isAuthenticated = computed(() => !!activeToken.value);

  const logIn = async (user: User) => {
    const token = await api.customFetch<{ token: string }>("users/login", {
      method: "POST",
      body: user,
    })

    activeToken.value = token.token;

    return true;
  };
  
  const logOut = async() => {
    activeToken.value = undefined;
    location.replace('/login');
  };

  const fetchWithToken = async <T>(
    url: string,
    options?: NitroFetchOptions<NitroFetchRequest>
  ) => {
    return await api.customFetch<T>(url, {
        ...options, 
        headers: {
            Authorization: 'Bearer ' + activeToken.value,
            ...options?.headers
        }
    });
  };

  return { logIn, logOut, isAuthenticated, fetchWithToken };
};