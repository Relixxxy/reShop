const {
  REACT_APP_CATALOG_URL = "http://www.alevelwebsite.com:5000/api/v1/catalogbff",
  REACT_APP_AUTHORITY = "http://www.alevelwebsite.com:5002",
  REACT_APP_CLIENT_ID = "react-client",
  REACT_APP_REDIRECT_URI = "http://www.alevelwebsite.com/signin-oidc",
  REACT_APP_RESPONSE_TYPE = "code",
  REACT_APP_SCOPE = "openid profile mvc",
  REACT_APP_POST_LOGOUT_REDIRECT_URI = "http://www.alevelwebsite.com/",
} = process.env;

interface Config {
  catalog_url: string;
  authority: string;
  client_id: string;
  redirect_uri: string;
  response_type: string;
  scope: string;
  post_logout_redirect_uri: string;
}

export const config: Config = {
  catalog_url: REACT_APP_CATALOG_URL,
  authority: REACT_APP_AUTHORITY,
  client_id: REACT_APP_CLIENT_ID,
  redirect_uri: REACT_APP_REDIRECT_URI,
  response_type: REACT_APP_RESPONSE_TYPE,
  scope: REACT_APP_SCOPE,
  post_logout_redirect_uri: REACT_APP_POST_LOGOUT_REDIRECT_URI,
};
