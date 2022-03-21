import React from 'react';
import 'react-quill/dist/quill.snow.css';
import 'react-quill/dist/quill.bubble.css';
import * as Quill from "quill";
import ReactQuill from 'react-quill';

export interface IEditorConvertToHTMLProps {
    onChangeHandler?: (content: string) => void;
    text: string;
}
export interface IEditorConvertToHTMLState {
    text: string;
}

class EditorConvertToHTML extends React.Component<IEditorConvertToHTMLProps, IEditorConvertToHTMLState> {

    constructor(props: any) {
        super(props)
        this.state = { text: '' } // You can also pass a Quill Delta here
        this.handleChange = this.handleChange.bind(this)
      }
     
   
    handleChange(content: string, delta: any, source: Quill.Sources, editor: any): void {
        this.props.onChangeHandler && this.props.onChangeHandler(content);
    }
    
    render() {

        return (
            <ReactQuill 
                style={{ borderTop: 'none'}}
                value={this.props.text}
                onChange={this.handleChange} 
                formats={[
                    'bold', 'italic', 'underline',
                    'list', 'bullet',
                    'align',
                    'color', 'background'
                ]}
                modules={
                    {
                        toolbar: [
                          ['bold', 'italic', 'underline'],
                          [{'list': 'ordered'}, {'list': 'bullet'}],
                          [{ 'align': [] }],
                          [{ 'color': [] }, { 'background': [] }],
                          ['clean']
                        ]
                    }
                }
            />
        )
    }
  }

  export default EditorConvertToHTML;