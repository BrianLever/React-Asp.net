import { IScreenListResponseItem } from '../../actions/screen';
import axios from  '../axios';

const postScreenList = async (): Promise<{ Items: IScreenListResponseItem[] }> => {
   const currentDateStamp = new Date().toISOString();
   return await axios.instance.post('screen/search', {
      "Location": null,
      "StartDate": "2020-10-01",
      "EndDate": currentDateStamp,
      "FirstName": null,
      "LastName": null,
      "ScreeningResultID": null,
      "StartRowIndex": 0,
      "MaximumRows": 100
    });
}

export default postScreenList;