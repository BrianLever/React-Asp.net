import axios from  '../axios';

const updateScreenProfileFrequencyList = async (id: number, props: { Items: Array<any>} )  => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`screeningprofile/${id}/frequency`, {
        ...replace,
    });
}

export default updateScreenProfileFrequencyList;