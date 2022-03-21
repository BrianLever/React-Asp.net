import { ILicenseKeysResponseItem } from '../../actions/license-keys';
import axios from  '../axios';

const getLicenseKeys = async (): Promise<Array<ILicenseKeysResponseItem>> => {
   return await axios.instance.get('systemtools/license');
}

export default getLicenseKeys;