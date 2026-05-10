using GenJsonConfig.Attributes;
using MediaDeck.Composition.Stores;

[assembly: RegisterJsonConfigWrapper(typeof(ReactiveProperty<>), typeof(ReactivePropertyAdapter<>))]