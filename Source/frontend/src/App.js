import * as React from "react";
import NotFoundPage from "./pages/NotFoundPage";
import "./index.css";
import { Route, Switch } from "react-router-dom/cjs/react-router-dom.min";
import { useApi } from "./context/ApiContext";
import LoadSpinner from "./components/LoadSpinner";
import Lancamentos from "./pages/Lancamentos";
import Login from "./pages/Login";
import MeusLancamentos from "./pages/MeusLancamentos";
import Relatorio from "./pages/Relatorio";

function App() {
  const { isAuthenticated, load } = useApi();
  if (!isAuthenticated) {
    return (
      <>
        <Switch>
          <Route exact path="/*">
            <Login />
          </Route>
        </Switch>
        <LoadSpinner isLoading={load} />
      </>
    );
  }
  return (
    <div>
      <Switch>
        <Route exact path="/">
          <Lancamentos />
        </Route>
        <Route exact path="/lancamentos">
          <Lancamentos />
        </Route>
        <Route exact path="/meus-lancamentos">
          <MeusLancamentos />
        </Route>

        <Route exact path="/relatorio">
          <Relatorio />
        </Route>

        <Route exact path="/login">
          <Login />
        </Route>
        <Route exact path="*">
          <NotFoundPage />
        </Route>
      </Switch>
      <LoadSpinner isLoading={load} />
    </div>
  );
}

export default App;
