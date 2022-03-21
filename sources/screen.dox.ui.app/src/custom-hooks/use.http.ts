import React from 'react';
import { EHttpMethods } from '../common-types/http.type';

export interface IUseHttpProps {
    url: string;
    method: EHttpMethods;
    headers?: { [k: string]: string };
    body: any;
    errorHandler?: (error?: Error) => void;
}

export  interface IUseHttpResponse {
    error: Error | null;
    isLoading: boolean;
    sendRequest: () => Promise<any>;
}

const useHttp = async (httpProps: IUseHttpProps) => {
    const [isLoading, setLoadingStatus] = React.useState(false);
    const [error, setErrot] = React.useState(null);

    const sendRequest = async (): Promise<IUseHttpResponse> => {
        const { url, method, headers, body = {}, errorHandler } = httpProps || {};
        setLoadingStatus(true);
        setErrot(null);
        try {
            const response = await fetch(url, {
                ...httpProps,
                method,
                headers,
                body: JSON.stringify(body),
            });

            if (!response.ok) {
                throw new Error('Request failed.');
            }

            const data = await response.json();
            return data;
        } catch (error) {
            setErrot(error);
            errorHandler && errorHandler(error);
        }
        setLoadingStatus(false);

        return { isLoading, error, sendRequest }
    };
};

export default useHttp;