import { useEffect, useState } from "react";
import Table from "../components/Table";
import ConsolidadoService from "../services/ConsolidadoService";
import Helper from "../helpers/Helper";
import { useApi } from "../context/ApiContext";
import DateFilterButtons from "../components/DateFilterButtons";
import SearchDate from "../components/SearchDate";
import HeaderPage from "../components/HeaderPage";

function MeusLancamentos() {
  const [dados, setDados] = useState([]);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [contas, setContas] = useState([]);
  const [contaSelect, setContaSelect] = useState("");
  const { setLoad, user } = useApi();

  const obterDetalhes = async (id) => {
    setDados(await ConsolidadoService.obterDetalhesConsolidados(id));
  };

  const obterContas = async () => {
    if (user?.nameidentifier) {
      const contas = await ConsolidadoService.ObterContasBancarias(
        user?.nameidentifier
      );
      setContas(contas);
      return contas;
    }
    return null;
  };

  useEffect(() => {
    inicializar();
  }, [user?.nameidentifier, window.location.href]);

  async function inicializar() {
    setLoad(true);
    const res = await obterContas();
    if (res && res.length > 0) {
      await ConsolidadoService.obterDetalhesConsolidadosPorData(
        res[0].id,
        startDate,
        endDate
      );
      setContaSelect(res[0].id);
      obterDetalhes(res[0].id);
    }
    setLoad(false);
  }

  const handleSearch = async () => {
    setLoad(true);
    const detalhesConsolidados =
      await ConsolidadoService.obterDetalhesConsolidadosPorData(
        contaSelect,
        startDate,
        endDate
      );
    setDados(detalhesConsolidados);
    setLoad(false);
  };

  const handleAutoFill = (days) => {
    const today = new Date();
    setEndDate(today.toISOString().split("T")[0]);
    today.setDate(today.getDate() - days);
    setStartDate(today.toISOString().split("T")[0]);
  };

  const tableColumns = [
    {
      key: "Id",
      label: "Cod. Identificação",
      className: "text-cente w-2",
      component: ({ id }) => (
        <span title={id} alt={id}>
          {id.substr(0, 8)}...
        </span>
      ),
    },
    {
      key: "descricao",
      label: "Descrição",
      className: "text-start pl-8 font-semibold",
    },
    {
      key: "valor",
      label: "Valor do Lançamento",
      className: "text-center w-[200px]",
      component: ({ valor, tipo }) => (
        <span
          className={`text-md font-semibold ${
            tipo === "1" ? "text-red-600" : "text-green-600"
          }`}
        >
          {Helper.formatMoney(valor)}
        </span>
      ),
    },
    {
      key: "tipo",
      label: "Tipo do lançamento",
      className: "text-center w-[200px]",
      component: ({ tipo }) => (
        <span
          className={`text-md font-semibold ${
            tipo === "1" ? "text-red-600" : "text-green-600"
          }`}
        >
          {Helper.getTypeCreditOrDebit(tipo)}
        </span>
      ),
    },

    {
      key: "data",
      label: "Data",
      className: "text-center w-[200px]",
      component: ({ data }) => (
        <span className="font-semibold">{Helper.formatDateColumn(data)}</span>
      ),
    },
  ];

  return (
    <div className="flex flex-center justify-center">
      <div className="container">
        <HeaderPage
          title="Meus Lançamentos"
          description="Todos os seus lançamentos de acordo com a conta bancária."
        />
        <SearchDate
          setContaSelect={setContaSelect}
          setStartDate={setStartDate}
          setEndDate={setEndDate}
          startDate={startDate}
          endDate={endDate}
          handleSearch={handleSearch}
          contas={contas}
        />
        <DateFilterButtons handleAutoFill={handleAutoFill} />
        <div className="mt-3">
          <Table className={"w-full"} columns={tableColumns} data={dados} />
        </div>
      </div>
    </div>
  );
}

export default MeusLancamentos;
