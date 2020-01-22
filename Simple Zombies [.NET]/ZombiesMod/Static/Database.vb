System.ArgumentOutOfRangeException: Cannot deserialize WeaponHash type
Parameter name: type
   at Reflector.Disassembler.TypeConverter.GetReaderFunction(ITypeReference type, Int32& size)
   at Reflector.Disassembler.TypeConverter.<GetLiteralsFromBits>d__92.MoveNext()
   at Reflector.Disassembler.Instructions.CallInstruction.<>c__DisplayClass9_0.<ConvertHandleToArray>g__Decode|1(BlockExpression block, Int32 depth, <>c__DisplayClass9_1& )
   at Reflector.Disassembler.Instructions.CallInstruction.ConvertHandleToArray(NewTranslator translator, IRuntimeHandleExpression runtimeHandleExpression, IArrayCreateExpression expression)
   at Reflector.Disassembler.Instructions.CallInstruction.CheckForFrameworkMethods(NewTranslator translator, IMethodReference methodReference, Boolean& fail)
   at Reflector.Disassembler.Instructions.CallInstruction.AnalyzeExpression(NewTranslator translator, Object value)
   at Reflector.Disassembler.TranslatorBase.NodeAnalyzer.Analyze(CodeNode node)
   at Reflector.Disassembler.TranslatorBase.NodeAnalyzer.AnalyzeAll()
   at Reflector.Disassembler.TranslatorBase.NodeAnalyzer.AnalyzeAll(NewTranslator translator)
   at Reflector.Disassembler.NewTranslator.TranslateMethodDeclaration(IMethodDeclaration mD, IMethodBody mB)
   at Reflector.Disassembler.Disassembler.TransformMethodDeclaration(IMethodDeclaration value)
   at Reflector.CodeModel.Visitor.Transformer.TransformMethodDeclarationCollection(IMethodDeclarationCollection methods)
   at Reflector.Disassembler.Disassembler.TransformTypeDeclaration(ITypeDeclaration value)
   at Reflector.Application.Translator.TranslateTypeDeclaration(ITypeDeclaration value, Boolean memberDeclarationList, Boolean methodDeclarationBody)
   at Reflector.Application.FileDisassembler.WriteTypeDeclaration(ITypeDeclaration typeDeclaration, String sourceFile, ILanguageWriterConfiguration languageWriterConfiguration)
Namespace ZombiesMod.Static
End Namespace

