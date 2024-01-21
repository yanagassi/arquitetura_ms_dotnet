import * as React from "react";
import NotFoundPage from "./pages/NotFoundPage";
import "./index.css";
import { Route, Switch } from "react-router-dom/cjs/react-router-dom.min";
import { useApi } from "./context/ApiContext";
import LoadSpinner from "./components/LoadSpinner";
import Lancamentos from "./pages/Lancamentos";

function App() {
  const { loading } = useApi();
  return (
    <div>
      <Switch>
        <Route exact path="/">
          <p>Dashboard</p>
        </Route>
        <Route exact path="/lancamentos">
          <Lancamentos />
        </Route>
        <Route exact path="*">
          <NotFoundPage />
        </Route>
      </Switch>
      <LoadSpinner isLoading={loading} />
    </div>
  );
}

export default App;
