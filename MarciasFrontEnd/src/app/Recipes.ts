
import { Food } from './Components/model/Foods';

export const RecipesList:Food[] = [
    {
      img: "/foodsImg/frangofrito2.png", // alterado de imgPath para img
      name: "Frango",
      compliment: "com Batata", // alterado de description para compliment
      price: 21.50,
      portion: 2,
      ingredients: ["Frango", "Batata"],
      id: 0
    },
    {
      img: "/foodsImg/fitness2.png",
      name: "Marmita Fitness",
      compliment: "Salada e Macarrão",
      price: 21.50,
      portion: 1,
      ingredients: ["Salada", "Macarrao"],
      id: 1
    },
    {
      img: "/foodsImg/batata2.png",
      name: "Abóbora",
      compliment: "com Salada",
      portion: 1, 
      ingredients: ["Abobora", "Salada"],
      price: 21.50,
      id: 2
    },
    {
      img: "/foodsImg/pizza2.png",
      name: "Pizza",
      compliment: "de Calabresa",
      price: 21.50,
      portion: 4,
      ingredients: ["Pizza", "Calabresa"],
      id: 3
    }
  ];