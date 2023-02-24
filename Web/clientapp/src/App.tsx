import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { routes as appRoutes } from "setup/routes/index";
import Layout from "components/layout";

function App() {
  return (
    <Router>
      <Layout>
        <Routes>
          {appRoutes.map((route) => (
            <Route
              key={route.key}
              path={route.path}
              element={<route.component />}
            />
          ))}
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;
