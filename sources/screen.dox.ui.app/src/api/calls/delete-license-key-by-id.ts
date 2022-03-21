import axios from  '../axios';

const deleteLicenseKey = async (props: any): Promise<string> => {
    const replace = !!props? props: {};
    return await axios.instance.delete(`systemtools/license`, { data: replace });
}
export default deleteLicenseKey;