import { UserManager, WebStorageStateStore, User } from "oidc-client";

import { config } from "setup/config";

const userManager = new UserManager({
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  ...config,
});

interface HttpClientService {
  getUser: () => Promise<User | null>;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  isLoggedIn: () => Promise<boolean>;
  completeLogin: () => Promise<User | null>;
  completeLogout: () => Promise<void>;
}

export const httpClientService: HttpClientService = {
  getUser: async () => {
    const user = await userManager.getUser();
    return user;
  },
  login: () => userManager.signinRedirect(),
  logout: () => userManager.signoutRedirect(),
  isLoggedIn: async () => {
    const user = await userManager.getUser();
    return !!user && !user.expired;
  },
  completeLogin: async () => {
    const user = await userManager.signinRedirectCallback();
    return user;
  },
  completeLogout: async () => {
    await userManager.signoutRedirectCallback();
  },
};
