import { defineConfig } from "cypress";
import axios from "axios";

export default defineConfig({
  e2e: {
    setupNodeEvents(on, config) {
      on('task', {
        'db:emptyDataSet': async () => {
          const {data} = await axios.post("http://localhost:7273/DataSet", {id: "d666ea00-b1be-4db6-8a9a-7911d7465b21"});
          return data;
        },
        'db:userOnlyDataSet': async () => {
          const {data} = await axios.post("http://localhost:7273/DataSet", {id: "07e63583-63b4-451b-9760-5d953a24c86e"});
          return data;
        },
        'db:userWithCreatedThreadSet': async () => {
          const {data} = await axios.post("http://localhost:7273/DataSet", {id: "d975383e-667b-4063-94ef-18eecbaa08f0"});
          return data;
        },
        'db:userWithCreatedThreadWithCreatedCommentSet': async () => {
          const {data} = await axios.post("http://localhost:7273/DataSet", {id: "bf27df9c-f97d-4ffe-acc0-221be5efa975"});
          return data;
        },
      })
    }
  }
});
