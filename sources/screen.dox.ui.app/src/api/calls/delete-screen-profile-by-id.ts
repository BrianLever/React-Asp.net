import axios from  '../axios';

const deleteScreenProfile = async (id: number)  => {
    return await axios.instance.delete(`screeningprofile/`+id);
}

export default deleteScreenProfile;