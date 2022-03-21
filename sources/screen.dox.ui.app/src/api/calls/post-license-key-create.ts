import axios from  '../axios';

const createLicenseKey = async (props: any): Promise<any> => {
    const replace  = !!props? props: {};
   return await axios.instance.post('systemtools/license', replace);
}

export default createLicenseKey;