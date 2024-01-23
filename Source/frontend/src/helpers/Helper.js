function getCurrentDate() {
  const currentDate = new Date();
  const year = currentDate.getUTCFullYear();
  const month = String(currentDate.getUTCMonth() + 1).padStart(2, "0");
  const day = String(currentDate.getUTCDate()).padStart(2, "0");

  const formattedDate = `${year}-${month}-${day}`;
  return formattedDate;
}

function formatDateColumn(dateTimeString) {
  const options = {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
  };

  const date = new Date(dateTimeString);
  const formattedDate = date.toLocaleString("en-US", options);

  return formattedDate.replace(/(\d+)\D+(\d+)\D+(\d+)/, "$2/$1/$3");
}

function formatMoney(numero = 0, locale = "pt-BR", currency = "BRL") {
  return numero.toLocaleString(locale, {
    style: "currency",
    currency,
  });
}

function getTypeCreditOrDebit(tipo) {
  if (typeof tipo === "string") tipo = parseInt(tipo);
  switch (tipo) {
    case 0:
      return "Debito";
    case 1:
      return "Crédito";
  }
}

export default {
  getCurrentDate,
  formatDateColumn,
  formatMoney,
  getTypeCreditOrDebit,
};
