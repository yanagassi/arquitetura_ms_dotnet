import * as React from "react";
import NotFoundPage from "./pages/NotFoundPage";
import "./index.css";
import { Route, Switch } from "react-router-dom/cjs/react-router-dom.min";
import { useApi } from "./context/ApiContext";
import LoadSpinner from "./components/LoadSpinner";
import Lancamentos from "./pages/Lancamentos";
import Login from "./pages/Login";
import Relatorio from "./pages/Relatorio";

function App() {
  const { isAuthenticated } = useApi();
  if (!isAuthenticated) {
    return (
      <Switch>
        <Route exact path="/*">
          <Login />
        </Route>
      </Switch>
    );
  }
  return (
    <div>
      <Switch>
        <Route exact path="/">
          <p>Dashboard</p>
        </Route>
        <Route exact path="/lancamentos">
          <Lancamentos />
        </Route>
        <Route exact path="/relatorios">
          <Relatorio />
        </Route>
        <Route exact path="/login">
          <Login />
        </Route>
        <Route exact path="*">
          <NotFoundPage />
        </Route>
      </Switch>
      <LoadSpinner isLoading={false} />
    </div>
  );
}

export default App;
