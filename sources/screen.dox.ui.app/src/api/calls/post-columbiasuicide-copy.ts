import axios from  '../axios';


const postColumbiasuicideCopy = async (props: { ID: number }): Promise<number> => {
   const replace = !!props ? props : {};
   return await axios.instance.post('columbiasuicide/copy', {     
      ...replace
   });
}

export default postColumbiasuicideCopy;
