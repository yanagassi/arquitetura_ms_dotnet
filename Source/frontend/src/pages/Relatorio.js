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
  }, [user?.nameidentifier]);

  async function inicializar() {
    setLoad(true);
    handleAutoFill(30);
    const resContas = await obterContas();
    if (resContas && resContas.length > 0) {
      setContaSelect(resContas[0].id);

      await handleSearch(resContas[0].id);
    }
    setLoad(false);
  }

  const handleSearch = async (accountId = null) => {
    setLoad(true);

    const detalhesConsolidados =
      await ConsolidadoService.obterSaldoConsolidadoPorData(
        typeof accountId === "string" ? accountId : contaSelect,
        startDate,
        endDate
      );
    setDados(detalhesConsolidados);
    setLoad(false);
  };

  const getTotalValue = () => {
    return contas.filter((e) => e.id === contaSelect)[0];
  };

  const handleAutoFill = (days) => {
    const today = new Date();
    setEndDate(today.toISOString().split("T")[0]);
    today.setDate(today.getDate() - days);
    setStartDate(today.toISOString().split("T")[0]);
  };

  const tableColumns = [
    {
      key: "lancamentos",
      label: "Nº Transações Diarias",
      className: "text-start pl-8 font-semibold  w-[150px]",
      component: ({ lancamentos }) => <span>{lancamentos?.length ?? 0}</span>,
    },
    {
      key: "totalValue",
      label: "Valor Total do Dia",
      className: "text-center  ",

      component: ({ totalValue, tipo }) => (
        <span
          className={`text-md font-semibold ${
            tipo !== "1" ? "text-red-600" : "text-green-600"
          }`}
        >
          {Helper.formatMoney(totalValue)}
        </span>
      ),
    },

    {
      key: "tipo",
      label: "Tipo de Transação",
      className: "text-center  ",
      component: ({ tipo }) => (
        <span
          className={`text-md font-semibold ${
            tipo !== "1" ? "text-red-600" : "text-green-600"
          }`}
        >
          {Helper.getTypeCreditOrDebit(tipo)}
        </span>
      ),
    },

    {
      key: "date",
      label: "Data",
      className: "text-center",
      component: ({ date }) => (
        <span className="font-semibold">{Helper.formatDateColumn(date)}</span>
      ),
    },
  ];

  return (
    <div className="flex flex-center justify-center">
      <div className="container">
        <HeaderPage
          title="Relatório de Consolidados Diarios"
          description="Saldos de todos os dias consolidado."
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
          <th className="text-zinc-700 border-gray bg-gray">
            <td>
              <span className="p-10 pl-2 pr-2 text-md">
                Total em Conta Hoje:
              </span>
              <span className="pr-2 text-green-600 text-md">
                {Helper.formatMoney(getTotalValue()?.saldo)}
              </span>
            </td>
          </th>
          <Table className={"w-full"} columns={tableColumns} data={dados} />
        </div>
      </div>
    </div>
  );
}

export default MeusLancamentos;
