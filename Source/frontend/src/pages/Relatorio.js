import { useEffect, useState } from "react";
import Table from "../components/Table";
import ConsolidadoService from "../services/ConsolidadoService";
import Helper from "../helpers/Helper";

function Relatorio() {
  const [dados, setDados] = useState([]);

  const obterDetalhes = async (id) => {
    try {
      const detalhesConsolidados =
        await ConsolidadoService.obterDetalhesConsolidados(id);
      setDados(detalhesConsolidados);
      console.log(detalhesConsolidados);
    } catch (error) {
      console.error("Erro ao obter detalhes consolidados:", error);
    }
  };

  useEffect(() => {
    obterDetalhes("89bc5c51-016e-4d57-a6bd-f884dedf98a8");
  }, []);

  const tableColumns = [
    {
      key: "Id",
      label: "Id",
      className: "text-center w-2",
      component: ({ id }) => (
        <span title={id} alt={id}>
          {id.substr(0, 8)}...
        </span>
      ),
    },
    {
      key: "descricao",
      label: "Descrição",
      className: "text-center",
    },
    {
      key: "valor",
      label: "valor",
      className: "text-center",
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
      label: "Tipo",
      className: "text-center",
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
      className: "text-center",
      component: ({ data }) => <span>{Helper.formatDateTimeColumn(data)}</span>,
    },
  ];
  return (
    <div className="flex flex-center justify-center">
      <div className="container">
        <h2 className="text-2xl font-semibold mb-8">Consolidado</h2>
        <Table className={"w-full"} columns={tableColumns} data={dados} />
      </div>
    </div>
  );
}

export default Relatorio;
